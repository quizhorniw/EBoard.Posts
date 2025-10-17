using System.Reflection;
using SolarLab.EBoard.Posts.Application;
using SolarLab.EBoard.Posts.Infrastructure;
using SolarLab.EBoard.Posts.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

var apiGroup = app.MapGroup("/api");
app.MapEndpoints(apiGroup);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MigrateDb();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.AddStaticFiles();

app.UseExceptionHandler();

app.Run();

public partial class Program { }