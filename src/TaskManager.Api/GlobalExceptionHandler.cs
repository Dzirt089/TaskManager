using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Application.Contracts.Exceptions;

namespace TaskManager.Api;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly IProblemDetailsService _problemDetailsService;

	public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
	{
		_problemDetailsService = problemDetailsService;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var (statusCode, title, detail, extensions) = exception switch
		{
			ValidationException validationException => (
				StatusCodes.Status400BadRequest,
				"Validation failed.",
				"One or more validation errors occurred.",
				new Dictionary<string, object?>
				{
					["errors"] = validationException.Errors
						.GroupBy(x => x.PropertyName)
						.ToDictionary(
							group => group.Key,
							group => group.Select(x => x.ErrorMessage).ToArray())
				}),
			NotFoundException notFoundException => (
				StatusCodes.Status404NotFound,
				"Resource not found.",
				notFoundException.Message,
				new Dictionary<string, object?>()),
			ConflictException conflictException => (
				StatusCodes.Status409Conflict,
				"Conflict.",
				conflictException.Message,
				new Dictionary<string, object?>()),
			ArgumentException argumentException => (
				StatusCodes.Status400BadRequest,
				"Invalid request.",
				argumentException.Message,
				new Dictionary<string, object?>()),
			_ => (
				StatusCodes.Status500InternalServerError,
				"Unexpected error.",
				"An unexpected error occurred while processing the request.",
				new Dictionary<string, object?>())
		};

		httpContext.Response.StatusCode = statusCode;

		var problemDetails = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Detail = detail,
			Instance = httpContext.Request.Path
		};

		foreach (var (key, value) in extensions)
		{
			problemDetails.Extensions[key] = value;
		}

		return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
		{
			HttpContext = httpContext,
			ProblemDetails = problemDetails,
			Exception = exception
		});
	}
}
