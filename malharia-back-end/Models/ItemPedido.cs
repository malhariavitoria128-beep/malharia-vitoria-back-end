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
			public decimal ValorTotal { get; set; } // Quantidade * ValorUnitario

			// Armazena o binário da imagem no banco
			public byte[]? Imagem { get; set; }

			public Pedido Pedido { get; set; } = null!;
	
	}
}
