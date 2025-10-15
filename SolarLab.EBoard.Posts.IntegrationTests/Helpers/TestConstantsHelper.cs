namespace SolarLab.EBoard.Posts.IntegrationTests.Helpers;

public static class TestConstantsHelper
{
    public static readonly Guid TestPostId = Guid.Parse("c56c734b-47a2-4405-9ff4-b240361ff7ff");
    public const string TestPostTitle = "Test Post Title";
    public const string TestPostDescription = "Test Post Description";
    public static readonly Guid TestCategoryId = Guid.Parse("fb507f54-ac3f-4031-8997-88d5ca4d0ae5");
    public const decimal TestPostPrice = 99.99m;
    
    public const string TestCategoryName = "Test Category Name";
    
    public static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}