using MediatR;

namespace SolarLab.EBoard.Posts.Application.Categories.GetById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>;