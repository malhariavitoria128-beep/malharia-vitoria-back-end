using malharia_back_end.Dtos;
using malharia_back_end.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

				// Sucesso
				return Ok(new { success = true, token = result });
			}
			catch (Exception ex)
			{
				// Log centralizado, ex: Serilog
				Log.Error(ex, "Erro ao autenticar usuário {Email}", dto.Email);

				// Retorna erro consistente para o front
				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro inesperado no servidor.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

	}
}
