using malharia_back_end.Dtos.ClientesDtos;
using System.Threading.Tasks;

namespace malharia_back_end.Services.Interfaces
{
	public interface IClienteService
	{
		Task RegisterAsync(ClienteDto dto);
		Task<List<BuscaClienteDto>> GetAllAsync();
		Task<BuscaClienteDto> GetByIdAsync(int id);
		Task UpdateAsync(int id, ClienteDto dto);
		Task DeleteClienteAsync(int clienteId);
	}
}
