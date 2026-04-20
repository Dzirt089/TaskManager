using MediatR;

namespace TaskManager.Application.Tasks.Queries.GetTaskById;

public sealed record GetTaskByIdQuery(Guid Id) : IRequest<TaskItemDto>;
