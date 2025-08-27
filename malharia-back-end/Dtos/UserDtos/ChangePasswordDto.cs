using System.ComponentModel.DataAnnotations;

namespace malharia_back_end.Dtos.UserDtos
{
	public record ChangePasswordDto(
		[Required(ErrorMessage = "Nova senha é obrigatória")]
		[MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
		[MaxLength(30, ErrorMessage = "Senha não pode ter mais de 30 caracteres")]
		[DataType(DataType.Password)]
		string NewPassword
	);
}
