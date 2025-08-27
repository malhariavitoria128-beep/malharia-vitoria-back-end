using malharia_back_end.Dtos.PedidosDto;
using malharia_back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace malharia_back_end.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PedidoController : ControllerBase
	{
		private readonly IPedidoService _pedidoService;

		public PedidoController(IPedidoService pedidoService)
		{
			_pedidoService = pedidoService;
		}

		[HttpPost]
		public async Task<IActionResult> CriarPedido([FromBody] PedidoCriarDto dto)
		{
			try
			{
				if (dto.ClienteId <= 0)
					return BadRequest(new { success = false, message = "Cliente inválido." });

				// Pode ser uma lista vazia de itens
				var pedido = await _pedidoService.CriarPedidoAsync(dto);
				return Ok(pedido);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					success = false,
					message = "Erro ao criar pedido.",
					details = ex.Message
				});
			}
		}
	

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var pedido = await _pedidoService.GetByIdAsync(id);
			if (pedido == null)
				return NotFound();

			return Ok(pedido);
		}

		[HttpPut("{id}/adicionar-item")]
		public async Task<IActionResult> AdicionarItem(int id, [FromBody] ItemPedidoDto item)
		{
			try
			{
				await _pedidoService.AdicionarItensAsync(id, item);
				return Ok(new { message = "Item adicionado com sucesso." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}

		[HttpPut("{id}/data-entrega")]
		public async Task<IActionResult> AtualizarDataEntrega(int id, [FromBody] DateTime novaDataEntrega)
		{
			try
			{
				await _pedidoService.AtualizarDataEntregaAsync(id, novaDataEntrega);
				return Ok(new { message = "Item adicionado com sucesso." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}

	}

}