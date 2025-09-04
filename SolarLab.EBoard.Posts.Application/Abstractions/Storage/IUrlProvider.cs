namespace SolarLab.EBoard.Posts.Application.Abstractions.Storage;

public interface IUrlProvider
{
    string GetUrl(string fileName);
}