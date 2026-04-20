using Microsoft.EntityFrameworkCore;

using System.Reflection;

using TaskManager.Api;

using TaskManager.Application;
using TaskManager.Infrastructure.DependencyInjection;
using TaskManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHealthChecks();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

	if (File.Exists(xmlPath))
	{
		options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
	}
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Container"))
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();
	if (dbContext.Database.IsRelational())
	{
		await dbContext.Database.MigrateAsync();
	}
	else
	{
		await dbContext.Database.EnsureCreatedAsync();
	}

	await DbSeeder.SeedAsync(dbContext);
}

app.Run();
