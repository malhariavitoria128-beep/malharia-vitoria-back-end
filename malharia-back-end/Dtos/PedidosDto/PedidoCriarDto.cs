using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.PedidosDto
{
	public record PedidoCriarDto
	{
		[Required(ErrorMessage = "O cliente é obrigatório")]
		public int ClienteId { get; set; }

		// Pode iniciar vazio; itens serão adicionados depois
		public List<ItemPedidoDto> Itens { get; set; } = new List<ItemPedidoDto>();
	}
}
