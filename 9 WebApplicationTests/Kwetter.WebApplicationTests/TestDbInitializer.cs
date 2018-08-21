using Kwetter.Data;
using Kwetter.Data.Initializer;
using Kwetter.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kwetter.WebApplicationTests
{
    class TestDbInitializer : IDbInitializer
    {
        public async void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

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
                    UserName = "User",
                    Email = "user@test.nl",
                    EmailConfirmed = true
                };

                var moderator = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Moderator",
                    UserName = "Moderator",
                    Email = "branchmanager@test.nl",
                    EmailConfirmed = true,
                };

                var admin = new ApplicationUser
                {
                    FirstName = "Kwetter",
                    LastName = "Admin",
                    UserName = "Administrator",
                    Email = "admin@test.nl",
                    EmailConfirmed = true
                };

                if (!context.Users.Any())
                {
                    var password = "Welcome1!";

                    await userManager.CreateAsync(user, password);
                    await userManager.CreateAsync(moderator, password);
                    await userManager.CreateAsync(admin, password);
                    await context.SaveChangesAsync();

                    await userManager.AddToRoleAsync(user, UserRoles.User);
                    await userManager.AddToRoleAsync(moderator, UserRoles.Moderator);
                    await userManager.AddToRoleAsync(admin, UserRoles.Administrator);
                    await context.SaveChangesAsync();
                }
                #endregion

                #region kweets
                if (!context.Kweets.Any())
                {
                    await context.Kweets.AddRangeAsync(
                        new Kweet
                        {
                            UserId = user.Id,
                            User = user,
                            Date = new DateTime(2018, 1, 1),
                            Description = "first kweet"
                        },
                        new Kweet
                        {
                            UserId = user.Id,
                            User = user,
                            Date = new DateTime(2018, 1, 1),
                            Description = "second kweet"
                        });
                    await context.SaveChangesAsync();
                }
                #endregion
            }
        }
    }
}
