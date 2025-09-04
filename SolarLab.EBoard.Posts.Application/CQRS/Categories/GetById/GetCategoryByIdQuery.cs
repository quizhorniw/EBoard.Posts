using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>;