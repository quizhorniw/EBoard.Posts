using System.Reflection;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Commons;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;

namespace SolarLab.EBoard.Posts.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(PostReadModel).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(AppDbContext).Assembly;
    protected static readonly Assembly WebApiAssembly = typeof(Program).Assembly;
}