using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.ClientesDtos
{
	public record ClienteDto : IValidatableObject
	{
		[Required(ErrorMessage = "Nome é obrigatório")]
		[MaxLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
		public string Nome { get; set; } = string.Empty;

		[MaxLength(100, ErrorMessage = "incorreto")]
		public string? CPF { get; set; }

		[MaxLength(100, ErrorMessage = "incorreto")]
		public string? CNPJ { get; set; }

		[MaxLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
		public string? Email { get; set; }

		[MaxLength(20, ErrorMessage = "Telefone não pode ter mais de 20 caracteres")]
		public string? Telefone { get; set; }

		[MaxLength(10, ErrorMessage = "CEP não pode ter mais de 10 caracteres")]
		public string? Cep { get; set; }

		[MaxLength(250, ErrorMessage = "Endereço não pode ter mais de 250 caracteres")]
		public string? Endereco { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!string.IsNullOrEmpty(CPF) && !string.IsNullOrEmpty(CNPJ))
			{
				yield return new ValidationResult(
					"Informe apenas CPF ou CNPJ, não ambos",
					new[] { nameof(CPF), nameof(CNPJ) });
			}
			
		}
	}
}
