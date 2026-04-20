using MediatR;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand(
	string Title,
	string? Description,
	Guid TaskTypeId,
	DateTime? DueDateUtc) : IRequest<Guid>;
