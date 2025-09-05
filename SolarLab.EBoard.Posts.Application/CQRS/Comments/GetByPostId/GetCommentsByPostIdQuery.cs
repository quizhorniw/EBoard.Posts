using MediatR;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;

public sealed record GetCommentsByPostIdQuery(Guid PostId) : IRequest<IReadOnlyList<CommentReadModel>>;