namespace malharia_back_end.Models
{
	public class Pedido
	{
		public int Id { get; set; }
		public int ClienteId { get; set; }
		public DateTime DataPedido { get; set; } = DateTime.Now;
		public decimal ValorTotal { get; set; }
		public string NumeroPedido { get; set; } = null!;

		public Cliente Cliente { get; set; } = null!;
		public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
	}
}
