using MediatR;

namespace TaskManager.Application.TaskTypes.Commands.DeleteTaskType;

public sealed record DeleteTaskTypeCommand(Guid Id) : IRequest;
