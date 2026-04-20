using MediatR;

namespace TaskManager.Application.TaskTypes.Queries.GetTaskTypeById;

public sealed record GetTaskTypeByIdQuery(Guid Id) : IRequest<TaskTypeDto>;
