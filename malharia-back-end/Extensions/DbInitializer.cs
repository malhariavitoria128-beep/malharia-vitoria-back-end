using malharia_back_end.Data;
using malharia_back_end.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace malharia_back_end.Static
{
	public static class DbInitializer
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			try
			{
				using var scope = serviceProvider.CreateScope();
				var db = scope.ServiceProvider.GetRequiredService<Context>();

				db.Database.Migrate();

				if (!db.Users.Any(u => u.Role == "Admin"))
				{
					db.Users.Add(new User
					{
						Email = "admin@local",
						PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
						Role = "Admin",
						IsApproved = true
					});
				}

				// Cria User se não existir
				if (!db.Users.Any(u => u.Role == "User"))
				{
					db.Users.Add(new User
					{
						Email = "user@local",
						PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
						Role = "User",
						IsApproved = true
					});
				}

				db.SaveChanges();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Erro fatal ao aplicar migrations ou criar admin");
			}
		}
	}
}
