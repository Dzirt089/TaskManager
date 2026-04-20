using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, TaskListResponse>
{
	private readonly IApplicationDbContext _dbContext;

	public GetTasksQueryHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<TaskListResponse> Handle(GetTasksQuery request, CancellationToken cancellationToken)
	{
		var query = _dbContext.Tasks
			.AsNoTracking()
			.Include(x => x.TaskType)
			.AsQueryable();

		if (request.Status.HasValue)
		{
			query = query.Where(x => x.Status == request.Status.Value);
		}

		if (request.TaskTypeId.HasValue)
		{
			query = query.Where(x => x.TaskTypeId == request.TaskTypeId.Value);
		}

		var totalCount = await query.CountAsync(cancellationToken);

		var sortedQuery = request.SortBy switch
		{
			TaskSortBy.CreatedAtAsc => query.OrderBy(x => x.CreatedAtUtc),
			TaskSortBy.DueDateAsc => query.OrderBy(x => x.DueDateUtc.HasValue).ThenBy(x => x.DueDateUtc),
			TaskSortBy.DueDateDesc => query.OrderByDescending(x => x.DueDateUtc.HasValue).ThenByDescending(x => x.DueDateUtc),
			TaskSortBy.TitleAsc => query.OrderBy(x => x.Title),
			TaskSortBy.TitleDesc => query.OrderByDescending(x => x.Title),
			_ => query.OrderByDescending(x => x.CreatedAtUtc)
		};

		var items = await sortedQuery
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.Select(x => new TaskItemDto(
				x.Id,
				x.Title,
				x.Description,
				x.Status.ToString(),
				x.TaskTypeId,
				x.TaskType.Name,
				x.CreatedAtUtc,
				x.UpdatedAtUtc,
				x.DueDateUtc))
			.ToListAsync(cancellationToken);

		return new TaskListResponse(
			request.Page,
			request.PageSize,
			totalCount,
			items);
	}
}
