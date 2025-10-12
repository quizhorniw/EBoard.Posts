using SolarLab.EBoard.Posts.Domain.Commons;
using SolarLab.EBoard.Posts.Domain.ValueObjects;

namespace SolarLab.EBoard.Posts.Domain.Entities;

public class Post : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Price { get; private set; }
    public Guid UserId { get; private set; }
    public List<Image> Images { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Post(Guid userId, string title, string? description, Guid categoryId, decimal price, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        CategoryId = categoryId;
        Price = price;
        Images = [];
        CreatedAt = createdAt;
    }

    public void UpdateDetails(string title, string? description, Guid categoryId, decimal price)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }
        
        Title = title;
        Description = description;
        CategoryId = categoryId;
        Price = price;
    }

    public void AddImage(Image image) => Images.Add(image);
    
    public void DeleteImage(Image image) => Images.Remove(image);
}