using FluentValidation;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed class GetTasksQueryValidator : AbstractValidator<GetTasksQuery>
{
	public GetTasksQueryValidator()
	{
		RuleFor(x => x.Page)
			.GreaterThan(0);

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100);
	}
}
