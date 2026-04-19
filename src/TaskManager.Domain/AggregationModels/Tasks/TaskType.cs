namespace TaskManager.Domain.AggregationModels.Tasks;

public sealed class TaskType
{
	private TaskType()
	{
	}

	public TaskType(Guid id, string name)
	{
		Id = id == Guid.Empty ? Guid.NewGuid() : id;
		Rename(name);
	}

	public Guid Id { get; private set; }

	public string Name { get; private set; } = null!;

	public void Rename(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Task type name is required.", nameof(name));
		}

		Name = name.Trim();
	}
}
