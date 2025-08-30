namespace malharia_back_end.Models
{
	public class ItemPedido
	{
		public int Id { get; set; }
		public int PedidoId { get; set; }
		public string Descricao { get; set; } = null!;
		public int Quantidade { get; set; }
		public string? Tamanho { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal ValorTotal { get; set; }
		public string? Prioridade { get; set; }

		// 🔹 Propriedades opcionais
		public string? TemPintura { get; set; }
		public string? StatusPintura { get; set; }
		public string? TemBordado { get; set; }
		public string? StatusBordado { get; set; }
		public string? TemDtf { get; set; }
		public string? StatusDtf { get; set; }
		public string? TemSilk { get; set; }
		public string? StatusSilk { get; set; }

		// 🔹 Etapas obrigatórias com marcador
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

		public byte[]? Imagem { get; set; }

		public Pedido Pedido { get; set; } = null!;
	}



}
