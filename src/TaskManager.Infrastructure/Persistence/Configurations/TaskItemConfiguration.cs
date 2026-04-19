using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TaskManager.Domain.AggregationModels.Tasks;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
	public sealed class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
	{
		public void Configure(EntityTypeBuilder<TaskItem> builder)
		{
			builder.ToTable("tasks");
			builder.HasKey(t => t.Id);

			builder.Property(x => x.Title)
				.HasMaxLength(200)
				.IsRequired();

			builder.Property(x => x.Description)
				.HasMaxLength(2000);

			builder.Property(x => x.Status)
				.HasConversion<string>()
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.CreatedAtUtc)
				.IsRequired();

			builder.Property(x => x.UpdatedAtUtc);
			builder.Property(x => x.DueDateUtc);

			builder.HasOne(x => x.TaskType)
				.WithMany()
				.HasForeignKey(x => x.TaskTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => x.TaskTypeId);
			builder.HasIndex(x => x.Status);
			builder.HasIndex(x => x.CreatedAtUtc);
		}
	}
}
