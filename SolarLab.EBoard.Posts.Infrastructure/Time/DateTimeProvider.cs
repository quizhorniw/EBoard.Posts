using SolarLab.EBoard.Posts.Application.Abstractions.Time;

namespace SolarLab.EBoard.Posts.Infrastructure.Time;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}