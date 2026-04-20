using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public sealed class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
	public UpdateTaskCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Description)
			.MaximumLength(2000);

		RuleFor(x => x.TaskTypeId)
			.NotEmpty();
	}
}
