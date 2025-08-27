using malharia_back_end.Dtos.ClientesDtos;
using malharia_back_end.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace malharia_back_end.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClienteController : ControllerBase
	{
		private readonly IClienteService _svc;

		public ClienteController(IClienteService svc)
		{
			_svc = svc;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] ClienteDto dto)
		{
			try
			{
				await _svc.RegisterAsync(dto);

				return Ok(new
				{
					success = true,
					message = "Cliente cadastrado com sucesso.",
					data = (object?)null
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao registrar cliente {Nome}", dto.Nome);

				return StatusCode(500, new
				{
					success = false,
					message = ex.Message,
					details = ex.InnerException?.Message ?? ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] ClienteDto dto)
		{
			try
			{
				await _svc.UpdateAsync(id, dto);

				return Ok(new
				{
					success = true,
					message = "Cliente atualizado com sucesso.",
					data = (object?)null
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao atualizar cliente {Nome}", dto.Nome);

				return StatusCode(500, new
				{
					success = false,
					message = ex.Message,
					details = ex.InnerException?.Message ?? ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var clientes = await _svc.GetAllAsync();
				return Ok(clientes);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao buscar usuários aprovados.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpGet("get-by-id/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var cliente = await _svc.GetByIdAsync(id);

				if (cliente == null)
					return NotFound(new
					{
						success = false,
						message = "Cliente não encontrado.",
						data = (object?)null
					});

				return Ok(cliente);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					success = false,
					message = "Ocorreu um erro ao buscar o cliente.",
					details = ex.Message,
					data = (object?)null
				});
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCliente(int id)
		{
			try
			{
				Log.Information("Iniciando remoção do cliente ID {ClienteId}", id);

				await _svc.DeleteClienteAsync(id);

				Log.Information("Cliente ID {ClienteId} removido com sucesso", id);
				return Ok(new
				{
					success = true,
					message = "Cliente removido com sucesso.",
					userId = id
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao remover cliente ID {ClienteId}", id);
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
