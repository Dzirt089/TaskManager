using MediatR;

namespace TaskManager.Application.TaskTypes.Queries.GetTaskTypes;

public sealed record GetTaskTypesQuery() : IRequest<IReadOnlyCollection<TaskTypeDto>>;
