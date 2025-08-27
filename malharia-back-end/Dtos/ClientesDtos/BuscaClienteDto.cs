using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.ClientesDtos
{
	public record BuscaClienteDto
	{
		public int? Id { get; set; }
		public string? Nome { get; set; } 
		public string? CPF { get; set; }
		public string? CNPJ { get; set; }
		public string? Email { get; set; }
		public string? Telefone { get; set; }
		public string? Cep { get; set; }
		public string? Endereco { get; set; }

	}
}