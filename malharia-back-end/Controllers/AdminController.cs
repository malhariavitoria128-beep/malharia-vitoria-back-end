using malharia_back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace malharia_back_end.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IUserService _svc;
		public AdminController(IUserService svc) => _svc = svc;

		[HttpGet("pending")]
		public async Task<IActionResult> GetPending()
		{
			try
			{
				Log.Information("Iniciando busca por usuários pendentes");

				var list = await _svc.GetPendingUsersAsync();

				Log.Information("Busca por usuários pendentes finalizada. Total: {Count}", list.Count());

				return Ok(list.Select(u => new { u.UserId, u.Email, u.CreatedAt }));
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao buscar usuários pendentes");

				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao buscar usuários pendentes.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpPost("approve/{id}")]
		public async Task<IActionResult> Approve(int id)
		{
			try
			{
				Log.Information("Iniciando aprovação do usuário ID {UserId}", id);

				await _svc.ApproveUserAsync(id);

				Log.Information("Aprovação concluída para usuário ID {UserId}", id);
				return NoContent();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao aprovar usuário ID {UserId}", id);
				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao aprovar o usuário.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			try
			{
				Log.Information("Iniciando remoção do usuário ID {UserId}", id);

				await _svc.DeleteUserAsync(id);

				Log.Information("Usuário ID {UserId} removido com sucesso", id);
				return NoContent();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao remover usuário ID {UserId}", id);
				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao remover o usuário.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

	}
}
