using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Infrastructure.Data.config
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.RefreshTokenId);

            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired();

            builder.Property(rt => rt.ExpiryTime)
                   .IsRequired();

            builder.Property(rt => rt.IsRevoked)
                   .IsRequired();

            builder.Ignore(rt => rt.IsExpired); // Computed in code, not mapped to DB
            builder.HasOne(rt => rt.User)
       .WithMany(u => u.RefreshTokens) // ✅ NOT .WithMany()
       .HasForeignKey(rt => rt.UserId)
       .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
