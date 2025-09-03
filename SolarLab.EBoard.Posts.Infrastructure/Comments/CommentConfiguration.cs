using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Infrastructure.Comments;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.PostId).IsRequired();
        builder.HasIndex(c => c.PostId);

        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.Text).IsRequired().HasMaxLength(1000);
        
        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}