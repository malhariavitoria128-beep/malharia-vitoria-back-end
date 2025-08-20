using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.UserDtos
{	
	public record RegisterByAdminDto(
		[Required(ErrorMessage = "Nome é obrigatório")]
		[MinLength(2, ErrorMessage = "Nome deve ter pelo menos 2 caracteres")]
		[MaxLength(50, ErrorMessage = "Nome não pode ter mais de 50 caracteres")]
		[Display(Name = "Nome completo")]
		string Nome,

		[Required(ErrorMessage = "Email é obrigatório")]
		[EmailAddress(ErrorMessage = "Email inválido")]
		[MaxLength(30, ErrorMessage = "Email não pode ter mais de 30 caracteres")]
		[Display(Name = "Email")]
		string Email
		);
	
}
