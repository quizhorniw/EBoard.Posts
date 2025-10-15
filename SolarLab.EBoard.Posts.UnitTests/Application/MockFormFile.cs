using Microsoft.AspNetCore.Http;

namespace SolarLab.EBoard.Posts.UnitTests.Application;

public class MockFormFile : IFormFile
{
    public string Name { get; }
    public string FileName { get; }
    
    public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public string ContentType => "image/png";
    public string ContentDisposition { get; }
    public long Length => 1024;
    
    public Stream OpenReadStream() => new MemoryStream();
    
    public void CopyTo(Stream target)
    {
        throw new NotImplementedException();
    }

    public IHeaderDictionary Headers { get; } = new HeaderDictionary();
    public long ContentLength => Length;

    public MockFormFile(string name, string fileName)
    {
        Name = name;
        FileName = fileName;
    }
}