using Article_Project.DataLayer.Config;
using Microsoft.AspNetCore.Identity;
using Article_Project.Entities.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.DataLayer.Context
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<IdentityUser<string>>().ToTable("Users", "identity");
            builder.Entity<IdentityRole<string>>().ToTable("Roles", "identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");

            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(p => new { p.LoginProvider, p.ProviderKey });
            builder.Entity<IdentityUserRole<string>>()
                .HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<IdentityUserToken<string>>()
                .HasKey(p => new { p.UserId, p.LoginProvider, p.Name });


            builder.Entity<User>()
                .HasMany(p => p.Followings)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.Entity<Like>()
                .HasOne(p => p.User)
                .WithMany(p => p.Likes)
                .HasForeignKey(p => p.UserId)
                ;

            builder.Entity<Like>()
                .HasOne(p => p.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(p => p.PostId);

            builder.Entity<Follow>()
                .HasOne(p => p.User)
                .WithMany(p => p.Followings)
                .HasForeignKey(p => p.UserId);

            builder.Entity<Follow>()
                .HasOne(p => p.UserTo)
                .WithMany(p => p.Followers)
                .HasForeignKey(p => p.FollowTo);

            builder.Entity<Comment>()
                .HasOne<Comment>()
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.ReplyComment);

            ApplyConfigures(builder);

            //base.OnModelCreating(builder);
        }

        private static void ApplyConfigures(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new FollowConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new RoleConfig());
        }
    }
}
