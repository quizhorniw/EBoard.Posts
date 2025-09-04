using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Domain.ValueObjects;

public class Image : ValueObject
{
    public string FileName { get; private set; }
    public string MimeType { get; private set; }
    public long Size { get; private set; }
    
    public Image(string fileName, string mimeType, long size)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("FileName is required", nameof(fileName));
        }

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            throw new ArgumentException("MimeType is required", nameof(mimeType));
        }

        if (size <= 0)
        {
            throw new ArgumentException("Size must be positive", nameof(size));
        }

        FileName = fileName;
        MimeType = mimeType;
        Size = size;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FileName;
        yield return MimeType;
        yield return Size;
    }
}