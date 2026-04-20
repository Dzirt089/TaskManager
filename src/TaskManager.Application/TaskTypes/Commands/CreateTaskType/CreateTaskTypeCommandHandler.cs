using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;
using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Application.TaskTypes.Commands.CreateTaskType;

public sealed class CreateTaskTypeCommandHandler : IRequestHandler<CreateTaskTypeCommand, Guid>
{
	private readonly IApplicationDbContext _dbContext;

	public CreateTaskTypeCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Guid> Handle(CreateTaskTypeCommand request, CancellationToken cancellationToken)
	{
		var normalizedName = request.Name.Trim();

		var exists = await _dbContext.TaskTypes
			.AnyAsync(x => x.Name.ToLower() == normalizedName.ToLower(), cancellationToken);

		if (exists)
		{
			throw new ConflictException("Task type with the same name already exists.");
		}

		var taskType = new TaskType(Guid.NewGuid(), normalizedName);

		_dbContext.TaskTypes.Add(taskType);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return taskType.Id;
	}
}
