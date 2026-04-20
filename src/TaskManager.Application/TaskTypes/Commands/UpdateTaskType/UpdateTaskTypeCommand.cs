using MediatR;

namespace TaskManager.Application.TaskTypes.Commands.UpdateTaskType;

public sealed record UpdateTaskTypeCommand(Guid Id, string Name) : IRequest;
