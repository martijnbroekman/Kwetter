using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Kwetter.Models;
using Microsoft.AspNetCore.Identity;

namespace Kwetter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Kweet> Kweets { get; set; }
        public DbSet<HashTag> HashTags { get; set; }
        public DbSet<HashTagInKweet> HashTagsInKweets { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Mention> Mentions { get; set; }
        public DbSet<Follower> Followers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Mention>()
                .HasOne(m => m.User)
                .WithMany(u => u.Mentions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Following)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follower>()
                .HasOne(f => f.Follows)
                .WithMany(u => u.Followers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
