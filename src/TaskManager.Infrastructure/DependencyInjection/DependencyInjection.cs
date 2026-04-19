using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TaskManager.Application.Contracts.Interfaces;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.DependencyInjection
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Postgres")
				?? throw new InvalidOperationException("Connection string 'Postgres' was not found.");

			services.AddDbContext<TaskManagerDbContext>(options =>
				options.UseNpgsql(connectionString));

			services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<TaskManagerDbContext>());

			return services;
		}
	}
}
