using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.UserDtos
{
	public record LoginDto(
		[Required(ErrorMessage = "Email é obrigatório")]
		[EmailAddress(ErrorMessage = "Email inválido")]
		[MaxLength(30, ErrorMessage = "Email não pode ter mais de 30 caracteres")]
		string Email,

		[Required(ErrorMessage = "Senha é obrigatória")]
		[MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
		[MaxLength(30, ErrorMessage = "Senha não pode ter mais de 30 caracteres")]
		string Password
   );
}
