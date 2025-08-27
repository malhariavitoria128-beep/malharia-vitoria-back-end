namespace malharia_back_end.Models
{
	public class Cliente
	{
		public int Id { get; set; }
		public string Nome { get; set; } = null!;
		public string? CPF { get; set; }
		public string? CNPJ { get; set; }
		public string? Email { get; set; }
		public string? Telefone { get; set; }
		public string? CEP { get; set; }
		public string? Endereco { get; set; }
		public DateTime CriadoEm { get; set; } = DateTime.Now;
		public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
	}
}
