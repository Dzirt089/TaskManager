using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Exceptions;
using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
	private readonly IApplicationDbContext _dbContext;

	public DeleteTaskCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
	{
		var task = await _dbContext.Tasks
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (task is null)
		{
			throw new NotFoundException("Task was not found.");
		}

		_dbContext.Tasks.Remove(task);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
