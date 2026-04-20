using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
	private readonly IApplicationDbContext _dbContext;

	public UpdateTaskCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
	{
		var task = await _dbContext.Tasks
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (task is null)
		{
			throw new NotFoundException("Task was not found.");
		}

		var taskTypeExists = await _dbContext.TaskTypes
			.AnyAsync(x => x.Id == request.TaskTypeId, cancellationToken);

		if (!taskTypeExists)
		{
			throw new NotFoundException("Task type was not found.");
		}

		task.Update(request.Title, request.Description, request.TaskTypeId, request.Status, request.DueDateUtc);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
