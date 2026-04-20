namespace TaskManager.Application.Tasks;

public sealed record TaskListResponse(
	int Page,
	int PageSize,
	int TotalCount,
	IReadOnlyCollection<TaskItemDto> Items);
