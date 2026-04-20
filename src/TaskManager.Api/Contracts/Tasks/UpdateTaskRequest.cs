namespace TaskManager.Api.Contracts.Tasks;

public sealed record UpdateTaskRequest(
	string Title,
	string? Description,
	TaskManager.Domain.AggregationModels.Tasks.TaskStatus Status,
	Guid TaskTypeId,
	DateTime? DueDateUtc);
