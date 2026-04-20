using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.TaskTypes.Commands.UpdateTaskType;

public sealed class UpdateTaskTypeCommandHandler : IRequestHandler<UpdateTaskTypeCommand>
{
	private readonly IApplicationDbContext _dbContext;

	public UpdateTaskTypeCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Handle(UpdateTaskTypeCommand request, CancellationToken cancellationToken)
	{
		var taskType = await _dbContext.TaskTypes
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (taskType is null)
		{
			throw new NotFoundException("Task type was not found.");
		}

		var normalizedName = request.Name.Trim();
		var duplicateExists = await _dbContext.TaskTypes
			.AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == normalizedName.ToLower(), cancellationToken);

		if (duplicateExists)
		{
			throw new ConflictException("Task type with the same name already exists.");
		}

		taskType.Rename(normalizedName);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
