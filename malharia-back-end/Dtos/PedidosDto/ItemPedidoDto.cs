using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.PedidosDto
{
	public record ItemPedidoDto
	{
		[MaxLength(2000, ErrorMessage = "A descrição não pode ter mais de 1000 caracteres")]
		public string Descricao { get; set; } = null!;

		[Range(1, 10000000000000000000, ErrorMessage = "A quantidade incorreta")]
		public int Quantidade { get; set; }

		[MaxLength(100, ErrorMessage = "O tamanho não pode ter mais de 100 caracteres")]
		public string? Tamanho { get; set; }

		[Range(0.01, 10000000000000000000, ErrorMessage = "Valor incorreto")]
		public decimal ValorUnitario { get; set; }

		public string? Imagem { get; set; }
	}
}
