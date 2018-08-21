using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kwetter.Models;
using Kwetter.WebApplication.Services;
using Kwetter.Data;
using AspNet.Security.OpenIdConnect.Primitives;
using Kwetter.Data.Initializer;
using Kwetter.Repository.Patterns;
using Kwetter.Service;
using AutoMapper;
using Kwetter.WebApplication.Configuration;
using Kwetter.Repository;

namespace Kwetter.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            if (HostingEnvironment.EnvironmentName.Equals("Test"))
            {
                var dataBaseName = Guid.NewGuid().ToString();
                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(databaseName: dataBaseName);
                        options.UseOpenIddict();
                    });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    options.UseOpenIddict();
                });

                services.AddSwaggerDocumentation(HostingEnvironment);
            }

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            Mapper.Reset();
            services.AddAutoMapper(option => option.AddProfile(new AutoMapperConfig()));

            // Register OpenIddict services
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<ApplicationDbContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the token endpoint.
                options.EnableTokenEndpoint("/connect/token");

                // Enable the password and the refresh token flows.
                options.AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                // When request caching is enabled, authorization and logout requests
                // are stored in the distributed cache by OpenIddict and the user agent
                // is redirected to the same page with a single parameter (request_id).
                // This allows flowing large OpenID Connect requests even when using
                // an external authentication provider like Google, Facebook or Twitter.
                options.EnableRequestCaching();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // To add third part of JWT token
                options.UseJsonWebTokens();
                options.AddEphemeralSigningKey();
            });

            services.AddAuthentication()
                .AddCookie()
                .AddOAuthValidation();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkAsync, UnitOfWork>();

            // Repositories
            services.AddScoped<IKweetRepository, KweetRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHashTagRepository, HashTagRepository>();
            services.AddScoped<IRepositoryAsync<Like>, Repository<Like>>();
            services.AddScoped<IRepositoryAsync<Follower>, Repository<Follower>>();
            services.AddScoped<IHashTagInKweetRepository, HashTagInKweetRepository>();
            services.AddScoped<IMentionRepository, MentionRepository>();

            // Services
            services.AddScoped<IKweetService, KweetService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFollowerService, FollowerService>();
            services.AddScoped<IHashTagService, HashTagService>();
            services.AddScoped<IHashTagInKweetService, HashTagInKweetService>();
            services.AddScoped<IMentionService, MentionService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            dbInitializer.Initialize(app);

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                app.UseSwaggerDocumentation();
            }
            else
            {
                //Add production seed

                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
