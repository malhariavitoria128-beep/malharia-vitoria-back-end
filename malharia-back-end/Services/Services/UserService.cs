using malharia_back_end.Data;
using malharia_back_end.Models;
using malharia_back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace malharia_back_end.Services.Services
{
	public class UserService : IUserService
	{
		private readonly Context _db;
		private readonly IConfiguration _config;

		public UserService(Context db, IConfiguration config)
		{
			_db = db;
			_config = config;
		}

		public async Task<string?> AuthenticateAsync(string email, string password)
		{
			try
			{
				var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
				if (user == null)
					return "ERROR_NOT_FOUND";

				if (!user.IsApproved)
					return "ERROR_NOT_APPROVED";

				if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
					return "ERROR_CREDENTIALS";

				// Gerar token
				var jwt = _config.GetSection("Jwt");
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var claims = new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
					new Claim(ClaimTypes.Role, user.Role),
					new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
					new Claim("is_approved", user.IsApproved ? "true" : "false")
				};

				var expires = DateTime.UtcNow.AddDays(1);
				var token = new JwtSecurityToken(
					issuer: jwt["Issuer"],
					audience: jwt["Audience"],
					claims: claims,
					expires: expires,
					signingCredentials: creds
				);

				return new JwtSecurityTokenHandler().WriteToken(token);
			}
			catch (Exception ex)
			{
				// Aqui você pode logar o erro, ex: Serilog
				Log.Error(ex, "Erro ao autenticar usuário {Email}", email);

				// Retorna valor especial para indicar erro inesperado
				return "ERROR_UNEXPECTED";
			}
		}

		public async Task RegisterAsync(string nome, string email, string password)
		{
			try
			{
				if (await _db.Users.AnyAsync(u => u.Email == email))
					throw new Exception("Email já cadastrado.");

				var user = new User
				{
					Nome = nome,
					Email = email,
					PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
					Role = "Operador",
					IsApproved = false
				};

				_db.Users.Add(user);
				await _db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao registrar usuário {Email}", email);
				throw; // relança para o controller tratar
			}
		}

		public async Task RegisterAdminAsync(string nome, string email)
		{
			try
			{
				if (await _db.Users.AnyAsync(u => u.Email == email))
					throw new Exception("Email já cadastrado.");

				var user = new User
				{
					Nome = nome,
					Email = email,
					PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
					Role = "Operador",
					IsApproved = true
				};

				_db.Users.Add(user);
				await _db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao registrar usuário {Email}", email);
				throw; // relança para o controller tratar
			}
		}

		public async Task ChangePasswordAsync(int userId, string newPassword)
		{
			try
			{
				var user = await _db.Users.FindAsync(userId)
						   ?? throw new Exception("Usuário não encontrado.");

				user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
				await _db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao alterar senha do usuário ID {UserId}", userId);
				throw; // relança para o controller tratar
			}
		}

		public async Task ChangeRoleAsync(int userId, string role)
		{
			try
			{
				var user = await _db.Users.FindAsync(userId)
						   ?? throw new Exception("Usuário não encontrado.");

				// Bloquear superusuário
				if (user.Email == "admin@local")
					throw new Exception("Não é permitido alterar a role do superusuário.");

				user.Role = role;
				await _db.SaveChangesAsync();

				Log.Information("Role alterada com sucesso para {UserId}", userId);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao alterar nível do usuário ID {UserId}", userId);
				throw; // relança para o controller tratar
			}
		}

		public async Task<IEnumerable<User>> GetPendingUsersAsync()
		{
			try
			{
				var users = await Task.FromResult(_db.Users.Where(u => !u.IsApproved).AsEnumerable());
				return users;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao buscar usuários pendentes");
				throw; // relança para o controller tratar
			}
		}

		public async Task<IEnumerable<User>> GetApprovedUsersAsync()
		{
			try
			{
				var users = await Task.FromResult(_db.Users.Where(u => u.IsApproved).AsEnumerable());
				return users;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao buscar usuários aprovados");
				throw; // relança para o controller tratar
			}
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			try
			{
				var users = await Task.FromResult(_db.Users.AsEnumerable());
				return users;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao buscar usuários");
				throw; // relança para o controller tratar
			}
		}

		public async Task ApproveUserAsync(int userId)
		{
			try
			{
				var user = await _db.Users.FindAsync(userId)
						   ?? throw new Exception("Usuário não existe.");

				user.IsApproved = true;
				await _db.SaveChangesAsync();

				Log.Information("Usuário ID {UserId} aprovado com sucesso", userId);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao aprovar usuário ID {UserId}", userId);
				throw; // relança para o controller tratar
			}
		}

		public async Task DeleteUserAsync(int userId)
		{
			try
			{
				var user = await _db.Users.FindAsync(userId)
						   ?? throw new Exception("Usuário não encontrado.");

				// Bloquear superusuário
				if (user.Email == "admin@local")
					throw new Exception("Não é permitido deletar o superusuário.");

				_db.Users.Remove(user);
				await _db.SaveChangesAsync();

				Log.Information("Usuário ID {UserId} removido com sucesso", userId);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao remover usuário ID {UserId}", userId);
				throw; // relança para o controller tratar
			}
		}



	}
}
