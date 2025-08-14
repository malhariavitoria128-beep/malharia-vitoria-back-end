using malharia_back_end.Data;
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

	}
}
