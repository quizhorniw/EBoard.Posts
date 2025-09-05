using MediatR;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetAll;

public sealed record GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryReadModel>>;