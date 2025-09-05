using MediatR;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryReadModel?>;