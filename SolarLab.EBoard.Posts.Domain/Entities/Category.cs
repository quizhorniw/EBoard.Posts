using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Domain.Entities;

public sealed class Category : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid? ParentId { get; private set; }
    
    public Category(string name, Guid? parentId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Invalid category name.", nameof(name));
        }
        
        Id = Guid.NewGuid();
        Name = name;
        ParentId = parentId;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Invalid category name.", nameof(newName));
        }

        Name = newName;
    }

    public void SetParent(Guid? parentId)
    {
        ParentId = parentId;
    }
}