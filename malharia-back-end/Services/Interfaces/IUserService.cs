using malharia_back_end.Models;

namespace malharia_back_end.Services.Interfaces
{
	public interface IUserService
	{
		Task<string?> AuthenticateAsync(string email, string password);
		Task RegisterAsync(string nome, string email, string password);
		Task ChangePasswordAsync(int userId, string newPassword);
		Task<IEnumerable<User>> GetPendingUsersAsync();
		Task ApproveUserAsync(int userId);
		Task DeleteUserAsync(int userId);
	}
}
