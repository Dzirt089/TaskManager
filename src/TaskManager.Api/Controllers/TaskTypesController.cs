using MediatR;

using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.Contracts.TaskTypes;
using TaskManager.Application.TaskTypes;
using TaskManager.Application.TaskTypes.Commands.CreateTaskType;
using TaskManager.Application.TaskTypes.Commands.DeleteTaskType;
using TaskManager.Application.TaskTypes.Commands.UpdateTaskType;
using TaskManager.Application.TaskTypes.Queries.GetTaskTypeById;
using TaskManager.Application.TaskTypes.Queries.GetTaskTypes;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Предоставляет операции CRUD для справочных данных типа задачи.
/// </summary>
[ApiController]
[Route("api/task-types")]
public sealed class TaskTypesController : ControllerBase
{
	private readonly IMediator _mediator;

	public TaskTypesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Возвращает все типы задач.
	/// </summary>
	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyCollection<TaskTypeDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<IReadOnlyCollection<TaskTypeDto>>> GetAll(CancellationToken cancellationToken)
	{
		var taskTypes = await _mediator.Send(new GetTaskTypesQuery(), cancellationToken);
		return Ok(taskTypes);
	}

	/// <summary>
	/// Возвращает один тип задачи по идентификатору.
	/// </summary>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(TaskTypeDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TaskTypeDto>> GetById(Guid id, CancellationToken cancellationToken)
	{
		var taskType = await _mediator.Send(new GetTaskTypeByIdQuery(id), cancellationToken);
		return Ok(taskType);
	}

	/// <summary>
	/// Создает новый тип задачи.
	/// </summary>
	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	public async Task<ActionResult<Guid>> Create(CreateTaskTypeRequest request, CancellationToken cancellationToken)
	{
		var id = await _mediator.Send(new CreateTaskTypeCommand(request.Name), cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id }, id);
	}

	/// <summary>
	/// Обновляет существующий тип задачи.
	/// </summary>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Update(Guid id, UpdateTaskTypeRequest request, CancellationToken cancellationToken)
	{
		await _mediator.Send(new UpdateTaskTypeCommand(id, request.Name), cancellationToken);
		return NoContent();
	}

	/// <summary>
	/// Удаляет тип задачи, если он не используется в задачах.
	/// </summary>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _mediator.Send(new DeleteTaskTypeCommand(id), cancellationToken);
		return NoContent();
	}
}
