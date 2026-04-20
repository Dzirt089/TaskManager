using FluentValidation;

namespace TaskManager.Application.TaskTypes.Commands.UpdateTaskType;

public sealed class UpdateTaskTypeCommandValidator : AbstractValidator<UpdateTaskTypeCommand>
{
	public UpdateTaskTypeCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
	}
}
