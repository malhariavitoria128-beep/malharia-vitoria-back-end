using malharia_back_end.Dtos.UserDtos;
using malharia_back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace malharia_back_end.Controllers
{
	[Authorize(Roles = "Administrador")]
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

				return Ok(list.Select(u => new { u.UserId, u.Email, u.CreatedAt, u.Nome, u.Role, u.IsApproved }));
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

		[HttpGet("approved")]
		public async Task<IActionResult> GetApproved()
		{
			try
			{
				Log.Information("Iniciando busca por usuários aprovados");

				var list = await _svc.GetApprovedUsersAsync();

				Log.Information("Busca por usuários aprovados finalizada. Total: {Count}", list.Count());

				return Ok(list.Select(u => new { u.UserId, u.Email, u.CreatedAt, u.Nome, u.Role, u.IsApproved }));
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao buscar usuários aprovados");

				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao buscar usuários aprovados.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				Log.Information("Iniciando busca por todos usuários");

				var list = await _svc.GetAllUsersAsync();

				Log.Information("Busca por usuários finalizada. Total: {Count}", list.Count());

				return Ok(list.Select(u => new { u.UserId, u.Email, u.CreatedAt, u.Nome, u.Role, u.IsApproved }));
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao buscar usuários");

				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao buscar usuários.",
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
				return Ok(new
				{
					success = true,
					message = "Usuário aprovado com sucesso.",
					userId = id
				});
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
				return Ok(new
				{
					success = true,
					message = "Usuário removido com sucesso.",
					userId = id
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao remover usuário ID {UserId}", id);
				return StatusCode(500, new
				{
					success = false,
					message = ex.Message,
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpPost("register-by-admin")]
		public async Task<IActionResult> RegisterByAdmin([FromBody] RegisterByAdminDto dto)
		{
			try
			{
				await _svc.RegisterAdminAsync(dto.Nome, dto.Email);

				return Ok(new
				{
					success = true,
					message = "Registrado, senha inicial 123456",
					data = (object?)null
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao registrar usuário {Email}", dto.Email);

				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro inesperado no servidor.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpPut("change-role")]
		public async Task<IActionResult> ChangeRole(string role, int id)
		{
			var userId = id;
			var rolenewRole = role;
			
			try
			{
				Log.Information("Usuário {UserEmail} (ID: {UserId}) iniciou alteração de senha", userId);

				await _svc.ChangeRoleAsync(userId, role);

				Log.Information("Senha alterada com sucesso para {UserId}", userId);

				return Ok(new
				{
					success = true,
					message = "Nível alterado com sucesso.",
					data = (object?)null
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao alterar nível do usuário (ID: {UserId})", userId);

				return StatusCode(500, new
				{
					success = false,
					message = ex.Message,
					details = ex.Message,
					data = (object?)null
				});
			}
		}

	}
}
