namespace malharia_back_end.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public string Role { get; set; } = "User";
		public bool IsApproved { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
