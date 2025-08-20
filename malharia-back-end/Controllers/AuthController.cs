using malharia_back_end.Dtos.UserDtos;
using malharia_back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace malharia_back_end.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _svc;
		public AuthController(IUserService svc) => _svc = svc;

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			try
			{
				var result = await _svc.AuthenticateAsync(dto.Email, dto.Password);

				if (result == "ERROR_NOT_FOUND")
					return Unauthorized(new { success = false, message = "Usuário não existe.", data = (object?)null });

				if (result == "ERROR_CREDENTIALS")
					return Unauthorized(new { success = false, message = "Credenciais inválidas.", data = (object?)null });

				if (result == "ERROR_NOT_APPROVED")
					return Unauthorized(new { success = false, message = "Usuário não aprovado.", data = (object?)null });

				return Ok(new { success = true, token = result });
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao autenticar usuário {Email}", dto.Email);

				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro inesperado no servidor.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			try
			{
				await _svc.RegisterAsync(dto.Nome, dto.Email, dto.Password);

				return Ok(new
				{
					success = true,
					message = "Registrado. Aguarde aprovação do admin.",
					data = (object?)null
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao registrar usuário {Email}", dto.Email);

				return StatusCode(500, new
				{
					success = false,
					message = ex.Message,
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[Authorize]
		[HttpPut("password")]
		public async Task<IActionResult> ChangeOwnPassword(ChangePasswordDto dto)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
			var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "Email-Não-Informado";

			try
			{
				Log.Information("Usuário {UserEmail} (ID: {UserId}) iniciou alteração de senha", userEmail, userId);

				await _svc.ChangePasswordAsync(userId, dto.NewPassword);

				Log.Information("Senha alterada com sucesso para {UserEmail}", userEmail);

				return Ok(new
				{
					success = true,
					message = "Senha alterada com sucesso.",
					data = (object?)null
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao alterar senha do usuário {UserEmail} (ID: {UserId})", userEmail, userId);

				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao alterar a senha.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}
	}

}
