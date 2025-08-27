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
	

		[HttpGet("{numeroPedido}")]
		public async Task<IActionResult> GetById(string numeroPedido)
		{
			var pedido = await _pedidoService.GetByIdAsync(numeroPedido);
			if (pedido == null)
				return NotFound();

			return Ok(pedido);
		}

		[HttpPut("{id}/adicionar-itens")]
		public async Task<IActionResult> AdicionarItens(int id, [FromBody] List<ItemPedidoDto> itens)
		{
			try
			{
				await _pedidoService.AdicionarItensAsync(id, itens);
				return Ok(new { message = "Itens adicionados com sucesso." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}
	}

}