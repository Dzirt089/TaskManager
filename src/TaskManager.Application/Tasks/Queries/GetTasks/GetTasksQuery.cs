using MediatR;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed record GetTasksQuery(
	int Page,
	int PageSize,
	TaskManager.Domain.AggregationModels.Tasks.TaskStatus? Status,
	Guid? TaskTypeId,
	TaskSortBy SortBy) : IRequest<TaskListResponse>;
