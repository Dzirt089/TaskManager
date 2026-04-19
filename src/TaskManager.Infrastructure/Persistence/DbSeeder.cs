using Microsoft.EntityFrameworkCore;

using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Infrastructure.Persistence
{
	public static class DbSeeder
	{
		public static readonly Guid BugId = Guid.Parse("11111111-1111-1111-1111-111111111111");
		public static readonly Guid FeatureId = Guid.Parse("22222222-2222-2222-2222-222222222222");
		public static readonly Guid ImprovementId = Guid.Parse("33333333-3333-3333-3333-333333333333");

		public static async Task SeedAsync(TaskManagerDbContext context, CancellationToken token = default)
		{
			var predefinedTypes = new[]
			{
				new TaskType(BugId, "Bug"),
				new TaskType(FeatureId, "Feature"),
				new TaskType(ImprovementId, "Improvement")
			};

			foreach (var taskType in predefinedTypes)
			{
				var existsTaskType = await context.TaskTypes
					.FirstOrDefaultAsync(x => x.Id == taskType.Id, token);

				if (existsTaskType is null)
					await context.TaskTypes.AddAsync(taskType, token);
				else
					if (IsRenamed(taskType, existsTaskType))
						existsTaskType.Rename(taskType.Name);
			}

			await context.SaveChangesAsync(token);
		}

		private static bool IsRenamed(TaskType taskType, TaskType existsTaskType)
		{
			return !string.Equals(existsTaskType.Name, taskType.Name, StringComparison.Ordinal);
		}
	}
}
