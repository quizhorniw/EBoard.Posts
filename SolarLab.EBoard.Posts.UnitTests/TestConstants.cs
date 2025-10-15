namespace SolarLab.EBoard.Posts.UnitTests;

public class TestConstants
{
    public static readonly Guid TestId = Guid.Parse("9ff9ef60-4b4c-47cd-b4c5-56b7408779b4");
    public static readonly Guid TestUserId = Guid.Parse("132b496c-aeed-49ef-a662-279435219fd6");
    public static readonly Guid TestPostId = Guid.Parse("d2eb359d-9d6c-4f20-b22e-4a9fc91844d5"); 
    public const string TestTitle = "Title";
    public const string TestDescription = "Description";
    public static readonly Guid TestCategoryId = Guid.Parse("996b8bcc-d09e-4818-b19a-1c458903f041");
    public const decimal TestPrice = 99.99m;
    public static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public const string TestText = "Text";
    public const string TestName = "Name"; 
    public static readonly Guid TestParentId = Guid.Parse("60e5f7b0-286e-41d5-b6bd-f905c35833f2"); 
}