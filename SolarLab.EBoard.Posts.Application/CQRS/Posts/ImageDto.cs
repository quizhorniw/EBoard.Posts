namespace SolarLab.EBoard.Posts.Application.CQRS.Posts;

public sealed record ImageDto(string Url, string MimeType, long Size);

public sealed class ImageDtoList
{
    public List<ImageDto> Images { get; set; }

    public ImageDtoList(List<ImageDto> images)
    {
        Images = images;
    }
}