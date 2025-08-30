using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.PedidosDto
{
	public record ItemPedidoDto
	{
		public int Id { get; set; }
		[MaxLength(2000)]
		public string Descricao { get; set; } = null!;

		[Range(1, long.MaxValue)]
		public int Quantidade { get; set; }

		[MaxLength(100)]
		public string? Tamanho { get; set; }

		[Range(0.01, double.MaxValue)]
		public decimal ValorUnitario { get; set; }

		public string? Imagem { get; set; }

		// 🔹 Propriedades opcionais
		public string? Prioridade { get; set; }
		public string? TemPintura { get; set; }
		public string? StatusPintura { get; set; }
		public string? TemBordado { get; set; }
		public string? StatusBordado { get; set; }
		public string? TemDtf { get; set; }
		public string? StatusDtf { get; set; }
		public string? TemSilk { get; set; }
		public string? StatusSilk { get; set; }

		// 🔹 Etapas obrigatórias marcadas
		public string? TemCorte { get; set; }
		public string? StatusCorte { get; set; }

		public string? TemCostura { get; set; }
		public string? StatusCostura { get; set; }

		public string? TemDobragem { get; set; }
		public string? StatusDobragem { get; set; }

		public string? TemConferencia { get; set; }
		public string? StatusConferencia { get; set; }

		public string? TemRetirada { get; set; }
		public string? StatusRetirada { get; set; }
	}




}
