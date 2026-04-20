using MediatR;

namespace TaskManager.Application.TaskTypes.Commands.CreateTaskType;

public sealed record CreateTaskTypeCommand(string Name) : IRequest<Guid>;
