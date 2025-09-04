using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Infrastructure.Posts;

internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.OwnsMany(p => p.Images, b =>
        {
            b.WithOwner().HasForeignKey("PostId");
            b.Property<int>("Id");
            b.HasKey("Id");
            
            b.Property(i => i.FileName).IsRequired();
            b.Property(i => i.MimeType).IsRequired();
            b.Property(i => i.Size).IsRequired();
        });
    }
}