namespace malharia_back_end.Dtos.PedidosDto
{
	public record AtualizarStatusItemDto
	{
		public string Campo { get; set; } = null!;
		public string Valor { get; set; } = null!;
	}
}
