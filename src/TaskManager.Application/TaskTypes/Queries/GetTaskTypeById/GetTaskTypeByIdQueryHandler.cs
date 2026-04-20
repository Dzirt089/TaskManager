using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.TaskTypes.Queries.GetTaskTypeById;

public sealed class GetTaskTypeByIdQueryHandler : IRequestHandler<GetTaskTypeByIdQuery, TaskTypeDto>
{
	private readonly IApplicationDbContext _dbContext;

	public GetTaskTypeByIdQueryHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<TaskTypeDto> Handle(GetTaskTypeByIdQuery request, CancellationToken cancellationToken)
	{
		var taskType = await _dbContext.TaskTypes
			.AsNoTracking()
			.Where(x => x.Id == request.Id)
			.Select(x => new TaskTypeDto(x.Id, x.Name))
			.FirstOrDefaultAsync(cancellationToken);

		return taskType ?? throw new NotFoundException("Task type was not found.");
	}
}
