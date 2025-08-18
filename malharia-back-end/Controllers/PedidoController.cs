using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace malharia_back_end.Controllers
{
	[Authorize(Roles = "Administrador,Operador")]
	[ApiController]
	[Route("api/[controller]")]
	public class PedidoController : ControllerBase
	{
		// Lista fake estática para teste
		private static readonly List<Pedido> Pedidos = new()
		{
			new Pedido { Id = 1, Cliente = "Cliente A", Produto = "Camisa Polo", Quantidade = 10, Status = "Em Produção" },
			new Pedido { Id = 2, Cliente = "Cliente B", Produto = "Calça Jeans", Quantidade = 5, Status = "Finalizado" },
			new Pedido { Id = 3, Cliente = "Cliente C", Produto = "Jaqueta", Quantidade = 2, Status = "Pendente" },
		};

		[HttpGet]
		public IActionResult GetPedidos()
		{
			return Ok(Pedidos);
		}

		[HttpGet("{id}")]
		public IActionResult GetPedido(int id)
		{
			var pedido = Pedidos.Find(p => p.Id == id);
			if (pedido == null)
				return NotFound();

			return Ok(pedido);
		}

		// Só Admin pode criar pedido (exemplo)
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult CriarPedido([FromBody] Pedido novoPedido)
		{
			novoPedido.Id = Pedidos.Count + 1;
			Pedidos.Add(novoPedido);
			return CreatedAtAction(nameof(GetPedido), new { id = novoPedido.Id }, novoPedido);
		}
	}

	public class Pedido
	{
		public int Id { get; set; }
		public string Cliente { get; set; } = null!;
		public string Produto { get; set; } = null!;
		public int Quantidade { get; set; }
		public string Status { get; set; } = null!;
	}
}
