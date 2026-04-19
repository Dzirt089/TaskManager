using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure.Persistence
{
	public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskManagerDbContext>
	{
		public TaskManagerDbContext CreateDbContext(string[] args)
		{
			var configurationRoot = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configurationRoot.GetConnectionString("Postgres")
				?? throw new InvalidOperationException("Connection string 'Postgres' was not found.");

			var builder = new DbContextOptionsBuilder<TaskManagerDbContext>();
			builder.UseNpgsql(connectionString);

			return new TaskManagerDbContext(builder.Options);
		}
	}
}
