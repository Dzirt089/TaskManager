using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.Tasks.Queries.GetTaskById;

public sealed class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItemDto>
{
	private readonly IApplicationDbContext _dbContext;

	public GetTaskByIdQueryHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<TaskItemDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
	{
		var task = await _dbContext.Tasks
			.AsNoTracking()
			.Include(x => x.TaskType)
			.Where(x => x.Id == request.Id)
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
			.FirstOrDefaultAsync(cancellationToken);

		return task ?? throw new NotFoundException("Task was not found.");
	}
}
