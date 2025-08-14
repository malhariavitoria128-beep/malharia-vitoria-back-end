namespace malharia_back_end.Services.Interfaces
{
	public interface IUserService
	{
		Task<string?> AuthenticateAsync(string email, string password);
	}
}
