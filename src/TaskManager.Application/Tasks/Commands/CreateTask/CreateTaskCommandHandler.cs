using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;
using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
	private readonly IApplicationDbContext _dbContext;

	public CreateTaskCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
	{
		var taskTypeExists = await _dbContext.TaskTypes
			.AnyAsync(x => x.Id == request.TaskTypeId, cancellationToken);

		if (!taskTypeExists)
		{
			throw new NotFoundException("Task type was not found.");
		}

		var task = new TaskItem(request.Title, request.Description, request.TaskTypeId, request.DueDateUtc);

		_dbContext.Tasks.Add(task);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return task.Id;
	}
}
