using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetAll;

public sealed record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;