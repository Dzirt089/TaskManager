namespace TaskManager.Domain.AggregationModels.Tasks;

public sealed class TaskItem
{
	private TaskItem()
	{
	}

	public TaskItem(
		string title,
		string? description,
		Guid taskTypeId,
		DateTime? dueDateUtc)
	{
		if (taskTypeId == Guid.Empty)
		{
			throw new ArgumentException("Task type id is required.", nameof(taskTypeId));
		}

		Id = Guid.NewGuid();
		SetTitle(title);
		SetDescription(description);
		TaskTypeId = taskTypeId;
		DueDateUtc = dueDateUtc;
		Status = TaskStatus.New;
		CreatedAtUtc = DateTime.UtcNow;
	}

	public Guid Id { get; private set; }

	public string Title { get; private set; } = null!;

	public string? Description { get; private set; }

	public TaskStatus Status { get; private set; }

	public Guid TaskTypeId { get; private set; }

	public TaskType TaskType { get; private set; } = null!;

	/// <summary>
	/// Дата создания задачи в UTC.
	/// </summary>
	public DateTime CreatedAtUtc { get; private set; }

	/// <summary>
	/// Дата последнего обновления задачи в UTC. Null, если задача не обновлялась после создания.
	/// </summary>
	public DateTime? UpdatedAtUtc { get; private set; }

	/// <summary>
	/// Срок сдачи задачи в UTC. Null, если срок сдачи не установлен.
	/// </summary>
	public DateTime? DueDateUtc { get; private set; }

	public void Update(
		string title,
		string? description,
		Guid taskTypeId,
		TaskStatus status,
		DateTime? dueDateUtc)
	{
		if (taskTypeId == Guid.Empty)
		{
			throw new ArgumentException("Task type id is required.", nameof(taskTypeId));
		}

		SetTitle(title);
		SetDescription(description);
		TaskTypeId = taskTypeId;
		Status = status;
		DueDateUtc = dueDateUtc;
		UpdatedAtUtc = DateTime.UtcNow;
	}

	private void SetTitle(string title)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			throw new ArgumentException("Title is required.", nameof(title));
		}

		Title = title.Trim();
	}

	private void SetDescription(string? description)
	{
		Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
	}
}
