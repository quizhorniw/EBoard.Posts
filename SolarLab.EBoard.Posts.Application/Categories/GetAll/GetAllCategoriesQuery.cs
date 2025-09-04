using MediatR;

namespace SolarLab.EBoard.Posts.Application.Categories.GetAll;

public sealed record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;