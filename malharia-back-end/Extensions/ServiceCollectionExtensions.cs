using malharia_back_end.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Serilog;

namespace malharia_back_end.Static
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			try
			{
				services.AddDbContext<Context>(options =>
					options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25)),
						mySqlOptions =>
						{
							mySqlOptions.EnableRetryOnFailure(2, TimeSpan.FromSeconds(5), null);
							mySqlOptions.CommandTimeout(3);
						})
				);

				var provider = services.BuildServiceProvider();
				var db = provider.GetRequiredService<Context>();
				db.Database.Migrate();
			}
			catch (MySqlException ex)
			{
				Log.Warning("Banco MySQL indisponível. Usando banco em memória como fallback: {Message}", ex.Message);
				services.AddDbContext<Context>(options =>
					options.UseInMemoryDatabase("FallbackDatabase"));
			}

			return services;
		}
		public static IServiceCollection AllowFirebase(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowFirebase", policy =>
				{
					policy.WithOrigins(
							"https://malharia-vitoria.web.app",  
							"https://malharia-vitoria.firebaseapp.com",
							"http://localhost:4200"
						)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
				});
			});

			return services;
		}

	}
}
