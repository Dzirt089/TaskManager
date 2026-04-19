using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Contracts.Interfaces;
using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Infrastructure.Persistence
{
	public sealed class TaskManagerDbContext : DbContext, IApplicationDbContext
	{

		public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
		{
		}

		public DbSet<TaskItem> Tasks => Set<TaskItem>();

		public DbSet<TaskType> TaskTypes => Set<TaskType>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerDbContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}
