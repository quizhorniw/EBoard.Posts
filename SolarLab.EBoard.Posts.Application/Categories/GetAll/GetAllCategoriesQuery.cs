using MediatR;

namespace SolarLab.EBoard.Posts.Application.Categories.GetAll;

public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;