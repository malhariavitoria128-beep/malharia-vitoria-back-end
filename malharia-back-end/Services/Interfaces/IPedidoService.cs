using malharia_back_end.Dtos.PedidosDto;

namespace malharia_back_end.Services.Interfaces
{
	public interface IPedidoService
	{
		Task<PedidoRespostaDto> CriarPedidoAsync(PedidoCriarDto dto);
		Task<PedidoRespostaDto> GetByIdAsync(string numeroPedido);
		Task AdicionarItensAsync(int pedidoId, List<ItemPedidoDto> itens);
	}
}
