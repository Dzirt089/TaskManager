using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
	public sealed class TaskTypeConfiguration : IEntityTypeConfiguration<TaskType>
	{
		public void Configure(EntityTypeBuilder<TaskType> builder)
		{
			builder.ToTable("task_types");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name)
				.HasMaxLength(100)
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();
		}
	}
}
