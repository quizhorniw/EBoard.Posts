namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class TestPostConstants
{
    public static readonly Guid TestUserId = Guid.Parse("132b496c-aeed-49ef-a662-279435219fd6");
    public const string TestTitle = "Title";
    public const string TestDescription = "Description";
    public static readonly Guid TestCategoryId = Guid.Parse("996b8bcc-d09e-4818-b19a-1c458903f041");
    public const decimal TestPrice = 99.99m;
    public static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}