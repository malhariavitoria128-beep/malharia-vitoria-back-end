namespace malharia_back_end.Dtos.PedidosDto
{
	public record PedidoRespostaDto
	{
		public int Id { get; set; }
		public string NumeroPedido { get; set; } = null!;
		public int ClienteId { get; set; }
		public string NomeCliente { get; set; }
		public DateTime DataPedido { get; set; }
		public decimal ValorTotal { get; set; }
		public List<ItemPedidoDto> Itens { get; set; } = new List<ItemPedidoDto>();
	}
}
