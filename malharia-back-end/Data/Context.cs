using malharia_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace malharia_back_end.Data
{
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options) { }
		public DbSet<User> Users { get; set; }
		public DbSet<Cliente> Clientes { get; set; }
		public DbSet<Pedido> Pedidos { get; set; }
		public DbSet<ItemPedido> ItensPedidos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();

			modelBuilder.Entity<Cliente>()
				.HasIndex(c => c.CPF)
				.IsUnique()
				.HasFilter("[CPF] IS NOT NULL"); // Só aplica unique para valores não nulos

			modelBuilder.Entity<Cliente>()
				.HasIndex(c => c.CNPJ)
				.IsUnique()
				.HasFilter("[CNPJ] IS NOT NULL"); // Só aplica unique para valores não nulos

			modelBuilder.Entity<Cliente>()
				.HasIndex(c => c.Nome)
				.IsUnique();

			modelBuilder.Entity<Cliente>()
				.Property(c => c.Nome)
				.IsRequired();

			modelBuilder.Entity<Pedido>()
				.HasOne(p => p.Cliente)
				.WithMany(c => c.Pedidos)
				.HasForeignKey(p => p.ClienteId)
				.OnDelete(DeleteBehavior.Restrict);

			// ItemPedido -> Pedido
			modelBuilder.Entity<ItemPedido>()
				.HasOne(i => i.Pedido)
				.WithMany(p => p.Itens)
				.HasForeignKey(i => i.PedidoId)
				.OnDelete(DeleteBehavior.Restrict);

			// Index opcional para otimizar buscas
			modelBuilder.Entity<ItemPedido>()
				.HasIndex(i => i.Descricao);
		}

	}
}
