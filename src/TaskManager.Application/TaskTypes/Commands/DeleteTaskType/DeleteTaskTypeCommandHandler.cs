using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.TaskTypes.Commands.DeleteTaskType;

public sealed class DeleteTaskTypeCommandHandler : IRequestHandler<DeleteTaskTypeCommand>
{
	private readonly IApplicationDbContext _dbContext;

	public DeleteTaskTypeCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Handle(DeleteTaskTypeCommand request, CancellationToken cancellationToken)
	{
		var taskType = await _dbContext.TaskTypes
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (taskType is null)
		{
			throw new NotFoundException("Task type was not found.");
		}

		var isInUse = await _dbContext.Tasks
			.AnyAsync(x => x.TaskTypeId == request.Id, cancellationToken);

		if (isInUse)
		{
			throw new ConflictException("Task type cannot be deleted while tasks reference it.");
		}

		_dbContext.TaskTypes.Remove(taskType);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
