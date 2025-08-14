using malharia_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace malharia_back_end.Data
{
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options) { }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();
		}

	}
}
