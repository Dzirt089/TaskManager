using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Interfaces;

namespace TaskManager.Application.TaskTypes.Queries.GetTaskTypes;

public sealed class GetTaskTypesQueryHandler : IRequestHandler<GetTaskTypesQuery, IReadOnlyCollection<TaskTypeDto>>
{
	private readonly IApplicationDbContext _dbContext;

	public GetTaskTypesQueryHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IReadOnlyCollection<TaskTypeDto>> Handle(GetTaskTypesQuery request, CancellationToken cancellationToken)
	{
		return await _dbContext.TaskTypes
			.AsNoTracking()
			.OrderBy(x => x.Name)
			.Select(x => new TaskTypeDto(x.Id, x.Name))
			.ToListAsync(cancellationToken);
	}
}
