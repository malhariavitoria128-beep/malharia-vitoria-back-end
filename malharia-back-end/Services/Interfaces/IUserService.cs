using malharia_back_end.Models;

namespace malharia_back_end.Services.Interfaces
{
	public interface IUserService
	{
		Task<string?> AuthenticateAsync(string email, string password);
		Task RegisterAsync(string nome, string email, string password);
		Task RegisterAdminAsync(string nome, string email);
		Task ChangePasswordAsync(int userId, string newPassword);
		Task ChangeRoleAsync(int userId, string role);
		Task<IEnumerable<User>> GetPendingUsersAsync();
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<IEnumerable<User>> GetApprovedUsersAsync();
		Task ApproveUserAsync(int userId);
		Task DeleteUserAsync(int userId);
	}
}
