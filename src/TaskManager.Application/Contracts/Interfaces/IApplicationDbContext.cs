using Microsoft.EntityFrameworkCore;

using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Application.Contracts.Interfaces
{
	public interface IApplicationDbContext
	{
		DbSet<TaskItem> Tasks { get; }
		DbSet<TaskType> TaskTypes { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
