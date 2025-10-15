using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SolarLab.EBoard.Posts.IntegrationTests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SubClaim = "12b1c71d-5641-49ff-9556-556b6336845a";
    private readonly string _role;
    
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<TestAuthHandlerOptions> handlerOptions)
        : base(options, logger, encoder)
    {
        _role = handlerOptions.Value.Role;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.NameIdentifier, SubClaim),
            new Claim(ClaimTypes.Role, _role)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}

public class TestAuthHandlerOptions
{
    public string Role { get; set; } = "User";
}