using FluentValidation;

namespace TaskManager.Application.TaskTypes.Commands.CreateTaskType;

public sealed class CreateTaskTypeCommandValidator : AbstractValidator<CreateTaskTypeCommand>
{
	public CreateTaskTypeCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
	}
}
