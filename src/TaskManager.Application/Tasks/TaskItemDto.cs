namespace TaskManager.Application.Tasks;

public sealed record TaskItemDto(
	Guid Id,
	string Title,
	string? Description,
	string Status,
	Guid TaskTypeId,
	string TaskTypeName,
	DateTime CreatedAtUtc,
	DateTime? UpdatedAtUtc,
	DateTime? DueDateUtc);
