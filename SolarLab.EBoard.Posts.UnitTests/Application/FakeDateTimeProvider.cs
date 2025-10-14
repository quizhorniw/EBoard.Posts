using SolarLab.EBoard.Posts.Application.Abstractions.Time;

namespace SolarLab.EBoard.Posts.UnitTests.Application;

public class FakeDateTimeProvider : IDateTimeProvider
{
    public FakeDateTimeProvider(DateTime dateTimeNow)
    {
        UtcNow = dateTimeNow;
    }

    public DateTime UtcNow { get; }
}