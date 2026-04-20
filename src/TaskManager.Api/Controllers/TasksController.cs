using MediatR;

using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.Contracts.Tasks;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Application.Tasks.Commands.DeleteTask;
using TaskManager.Application.Tasks.Commands.UpdateTask;
using TaskManager.Application.Tasks.Queries.GetTaskById;
using TaskManager.Application.Tasks.Queries.GetTasks;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Exposes CRUD operations for task items.
/// </summary>
[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
	private readonly IMediator _mediator;

	public TasksController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Returns a paged list of tasks with optional filtering and sorting.
	/// </summary>
	/// <param name="page">1-based page number.</param>
	/// <param name="pageSize">Number of items per page, up to 100.</param>
	/// <param name="status">Optional task status filter.</param>
	/// <param name="taskTypeId">Optional task type filter.</param>
	/// <param name="sortBy">Sort order for the returned items.</param>
	/// <param name="cancellationToken">Request cancellation token.</param>
	/// <returns>Paged task list response.</returns>
	[HttpGet]
	[ProducesResponseType(typeof(TaskListResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<TaskListResponse>> GetAll(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] TaskManager.Domain.AggregationModels.Tasks.TaskStatus? status = null,
		[FromQuery] Guid? taskTypeId = null,
		[FromQuery] TaskSortBy sortBy = TaskSortBy.CreatedAtDesc,
		CancellationToken cancellationToken = default)
	{
		var tasks = await _mediator.Send(new GetTasksQuery(page, pageSize, status, taskTypeId, sortBy), cancellationToken);
		return Ok(tasks);
	}

	/// <summary>
	/// Returns a single task by id.
	/// </summary>
	/// <param name="id">Task identifier.</param>
	/// <param name="cancellationToken">Request cancellation token.</param>
	/// <returns>Task details.</returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TaskItemDto>> GetById(Guid id, CancellationToken cancellationToken)
	{
		var task = await _mediator.Send(new GetTaskByIdQuery(id), cancellationToken);
		return Ok(task);
	}

	/// <summary>
	/// Creates a new task.
	/// </summary>
	/// <param name="request">Task payload.</param>
	/// <param name="cancellationToken">Request cancellation token.</param>
	/// <returns>Created task identifier.</returns>
	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<Guid>> Create(CreateTaskRequest request, CancellationToken cancellationToken)
	{
		var id = await _mediator.Send(
			new CreateTaskCommand(request.Title, request.Description, request.TaskTypeId, request.DueDateUtc),
			cancellationToken);

		return CreatedAtAction(nameof(GetById), new { id }, id);
	}

	/// <summary>
	/// Updates an existing task.
	/// </summary>
	/// <param name="id">Task identifier.</param>
	/// <param name="request">Updated task payload.</param>
	/// <param name="cancellationToken">Request cancellation token.</param>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken)
	{
		await _mediator.Send(
			new UpdateTaskCommand(id, request.Title, request.Description, request.Status, request.TaskTypeId, request.DueDateUtc),
			cancellationToken);

		return NoContent();
	}

	/// <summary>
	/// Deletes a task by id.
	/// </summary>
	/// <param name="id">Task identifier.</param>
	/// <param name="cancellationToken">Request cancellation token.</param>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);
		return NoContent();
	}
}
