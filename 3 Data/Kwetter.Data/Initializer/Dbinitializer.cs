using Kwetter.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Core;
using OpenIddict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kwetter.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        public async void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                var openIddictApplicationManager = serviceScope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                context.Database.Migrate();
                #region Clients
                var cancellationToken = new CancellationToken();
                if (await openIddictApplicationManager.FindByClientIdAsync("kwetter", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "kwetter",
                        DisplayName = "Kwetter",
                        RedirectUris = { new Uri("http://localhost:4200/") }
                    };

                    await openIddictApplicationManager.CreateAsync(descriptor, cancellationToken);
                }

                if (await openIddictApplicationManager.FindByClientIdAsync("postman", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") }
                    };

                    await openIddictApplicationManager.CreateAsync(descriptor, cancellationToken);
                }

                if (await openIddictApplicationManager.FindByClientIdAsync("swagger", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "swagger",
                        DisplayName = "swagger",
                        ClientSecret = "swagger"
                    };

                    await openIddictApplicationManager.CreateAsync(descriptor, cancellationToken);
                }
                #endregion

                #region UserRoles
                if (!context.UserRoles.Any())
                {
                    await roleManager.CreateAsync(new ApplicationRole(UserRoles.User));
                    await roleManager.CreateAsync(new ApplicationRole(UserRoles.Moderator));
                    await roleManager.CreateAsync(new ApplicationRole(UserRoles.Administrator));
                    await context.SaveChangesAsync();
                }
                #endregion

                #region Users
                var user = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "User",
                    UserName = "user",
                    Email = "user@test.nl",
                    EmailConfirmed = true
                };

                var moderator = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Moderator",
                    UserName = "manager",
                    Email = "manager@test.nl",
                    EmailConfirmed = true,
                };

                var admin = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Admin",
                    UserName = "admin",
                    Email = "admin@test.nl",
                    EmailConfirmed = true
                };

                var testUser1 = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Test1",
                    UserName = "test1",
                    Email = "test1@test.nl",
                    EmailConfirmed = true
                };

                var testUser2 = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Test2",
                    UserName = "test2",
                    Email = "test2@test.nl",
                    EmailConfirmed = true
                };

                var testUser3 = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Test3",
                    UserName = "test3",
                    Email = "test3@test.nl",
                    EmailConfirmed = true
                };

                if (!context.Users.Any())
                {
                    var password = "Welcome1!";

                    await userManager.CreateAsync(user, password);
                    await userManager.CreateAsync(moderator, password);
                    await userManager.CreateAsync(admin, password);
                    await userManager.CreateAsync(testUser1, password);
                    await userManager.CreateAsync(testUser2, password);
                    await userManager.CreateAsync(testUser3, password);
                    await context.SaveChangesAsync();

                    await userManager.AddToRoleAsync(user, UserRoles.User);
                    await userManager.AddToRoleAsync(moderator, UserRoles.Moderator);
                    await userManager.AddToRoleAsync(admin, UserRoles.Administrator);
                    await userManager.AddToRoleAsync(testUser1, UserRoles.User);
                    await userManager.AddToRoleAsync(testUser2, UserRoles.User);
                    await userManager.AddToRoleAsync(testUser3, UserRoles.User);
                    await context.SaveChangesAsync();
                }
                #endregion
            }
        }
    }
}
