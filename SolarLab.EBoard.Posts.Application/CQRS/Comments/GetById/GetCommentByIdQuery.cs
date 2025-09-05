using MediatR;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;

public sealed record GetCommentByIdQuery(Guid Id) : IRequest<CommentReadModel?>;