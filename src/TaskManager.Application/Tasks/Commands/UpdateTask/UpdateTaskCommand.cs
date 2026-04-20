using MediatR;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public sealed record UpdateTaskCommand(
	Guid Id,
	string Title,
	string? Description,
	TaskManager.Domain.AggregationModels.Tasks.TaskStatus Status,
	Guid TaskTypeId,
	DateTime? DueDateUtc) : IRequest;
