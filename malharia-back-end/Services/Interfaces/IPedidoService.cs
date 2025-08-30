using malharia_back_end.Dtos.PedidosDto;

namespace malharia_back_end.Services.Interfaces
{
	public interface IPedidoService
	{
		Task<PedidoRespostaDto> CriarPedidoAsync(PedidoCriarDto dto);
		Task<PedidoRespostaDto> GetByIdAsync(int id);
		Task<List<PedidoRespostaDto>> GetAsync();
		Task AdicionarItensAsync(int pedidoId, ItemPedidoDto itens);
		Task AtualizarDataEntregaAsync(int pedidoId, DateTime novaDataEntrega);
		Task AtualizarItemAsync(int itemId, AtualizarStatusItemDto dto);
	}
}
