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

				if (!db.Users.Any(u => u.Role == "Administrador"))
				{
					db.Users.Add(new User
					{
						Nome = "superusuario",
						Email = "admin@local",
						PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
						Role = "Administrador",
						IsApproved = true
					});
				}

				if (!db.Users.Any(u => u.Role == "Operador"))
				{
					db.Users.Add(new User
					{
						Nome = "operador",
						Email = "user@local",
						PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
						Role = "Operador",
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
