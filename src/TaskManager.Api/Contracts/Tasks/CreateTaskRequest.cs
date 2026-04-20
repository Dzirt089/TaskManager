namespace TaskManager.Api.Contracts.Tasks;

public sealed record CreateTaskRequest(
	string Title,
	string? Description,
	Guid TaskTypeId,
	DateTime? DueDateUtc);
