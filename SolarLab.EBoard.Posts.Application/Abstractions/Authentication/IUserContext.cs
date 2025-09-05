namespace SolarLab.EBoard.Posts.Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UserId { get; }
    
    bool IsInRole(string role);
}