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
	/// Возвращает постраничный список задач с дополнительной фильтрацией и сортировкой.
	/// </summary>
	/// <param name="page">Номер страницы от 1.</param>
	/// <param name="pageSize">Количество элементов на странице, до 100.</param>
	/// <param name="status">Необязательный фильтр по статусу задачи.</param>
	/// <param name="taskTypeId">Необязательный фильтр по типу задачи.</param>
	/// <param name="sortBy">Порядок сортировки возвращаемых элементов.</param>
	/// <param name="cancellationToken">Токен отмены запроса.</param>
	/// <returns>Ответ на постраничный список задач.</returns>
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
	/// Возвращает одну задачу по идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор задачи.</param>
	/// <param name="cancellationToken">Токен отмены запроса.</param>
	/// <returns>Детали задачи.</returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TaskItemDto>> GetById(Guid id, CancellationToken cancellationToken)
	{
		var task = await _mediator.Send(new GetTaskByIdQuery(id), cancellationToken);
		return Ok(task);
	}

	/// <summary>
	/// Создает новую задачу.
	/// </summary>
	/// <param name="request">Payload для создания задачи.</param>
	/// <param name="cancellationToken">Токен отмены запроса.</param>
	/// <returns>Идентификатор созданной задачи.</returns>
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
	/// Обновляет существующую задачу.
	/// </summary>
	/// <param name="id">Идентификатор задачи.</param>
	/// <param name="request">Обновленные данные задачи.</param>
	/// <param name="cancellationToken">Токен отмены запроса.</param>
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
	/// Удаляет задачу по идентификатору.
	/// 
	/// Возвращает `204 No Content`, потому что после успешного удаления
	/// дополнительное тело ответа уже не несёт полезной информации.
	/// </summary>
	/// <param name="id">Идентификатор задачи.</param>
	/// <param name="cancellationToken">Токен отмены запроса.</param>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);
		return NoContent();
	}
}
