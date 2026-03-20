using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoccerPro.Domain.Entities;
using SoccerPro.Infrastructure.Data.config;

namespace SoccerPro.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Explicitly configure one-to-one relationship between User and Person
            builder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne()
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Restrict); // or Cascade if you prefer

            builder.Entity<Person>().ToTable("People");
            builder.ApplyConfiguration(new RefreshTokenConfiguration());

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }

}
