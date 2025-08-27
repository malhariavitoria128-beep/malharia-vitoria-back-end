using malharia_back_end.Data;
using malharia_back_end.Dtos.PedidosDto;
using malharia_back_end.Models;
using malharia_back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace malharia_back_end.Services.Services
{
	public class PedidoService : IPedidoService
	{
		private readonly Context _db;

		public PedidoService(Context db)
		{
			_db = db;
		}

		private byte[]? Base64ParaByteArray(string? base64)
		{
			if (string.IsNullOrWhiteSpace(base64))
				return null;

			// Remove prefixo se existir (ex: "data:image/png;base64,")
			var index = base64.IndexOf("base64,");
			if (index >= 0)
			{
				base64 = base64.Substring(index + 7);
			}

			return Convert.FromBase64String(base64);
		}

		public async Task<PedidoRespostaDto> CriarPedidoAsync(PedidoCriarDto dto)
		{
			// Pega o ano atual
			var ano = DateTime.Now.Year;

			// Busca pedidos do ano atual para memória
			var pedidosAno = await _db.Pedidos
				.Where(p => p.DataPedido.Year == ano && p.NumeroPedido != null)
				.ToListAsync();

			// Calcula o maior sequencial
			int maiorSequencial = pedidosAno
				.Select(p => {
					var partes = p.NumeroPedido.Split('-');
					return int.TryParse(partes[1], out int seq) ? seq : 0;
				})
				.DefaultIfEmpty(0)
				.Max();

			// Próximo sequencial
			int sequencial = maiorSequencial + 1;

			// Número do pedido
			string numeroPedido = $"{ano}-{sequencial:0000}";

			// Cria pedido vazio
			var pedido = new Pedido
			{
				ClienteId = dto.ClienteId,
				DataPedido = DateTime.Now,
				ValorTotal = 0,
				NumeroPedido = numeroPedido
			};

			// Adiciona itens apenas se houver
			if (dto.Itens != null && dto.Itens.Any())
			{
				foreach (var itemDto in dto.Itens)
				{
					var item = new ItemPedido
					{
						Descricao = itemDto.Descricao,
						Quantidade = itemDto.Quantidade,
						Tamanho = itemDto.Tamanho,
						ValorUnitario = itemDto.ValorUnitario,
						ValorTotal = itemDto.Quantidade * itemDto.ValorUnitario,
						Imagem = Base64ParaByteArray(itemDto.Imagem)
					};

					pedido.Itens.Add(item);
				}

				pedido.ValorTotal = pedido.Itens.Sum(i => i.ValorTotal);
			}

			_db.Pedidos.Add(pedido);
			await _db.SaveChangesAsync();

			return new PedidoRespostaDto
			{
				Id = pedido.Id,
				NumeroPedido = numeroPedido,
				ClienteId = pedido.ClienteId,
				DataPedido = pedido.DataPedido,
				ValorTotal = pedido.ValorTotal,
				Itens = pedido.Itens.Select(i => new ItemPedidoDto
				{
					Descricao = i.Descricao,
					Quantidade = i.Quantidade,
					Tamanho = i.Tamanho,
					ValorUnitario = i.ValorUnitario,
					Imagem = i.Imagem != null ? Convert.ToBase64String(i.Imagem) : null
				}).ToList()
			};
		}

		public async Task<PedidoRespostaDto> GetByIdAsync(string numeroPedido)
		{
			var pedido = await _db.Pedidos
			.Include(p => p.Itens)
			.Include(p => p.Cliente)  // Inclui o cliente
			.FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido);

			if (pedido == null)
				return null!;

			//string numeroPedido = $"{pedido.DataPedido.Year}-{pedido.Id:0000}";

			var itens = pedido.Itens.Select(i => new ItemPedidoDto
			{
				Descricao = i.Descricao,
				Quantidade = i.Quantidade,
				Tamanho = i.Tamanho,
				ValorUnitario = i.ValorUnitario,
				Imagem = i.Imagem != null ? Convert.ToBase64String(i.Imagem) : null
			}).ToList();

			return new PedidoRespostaDto
			{
				Id = pedido.Id,
				NumeroPedido = pedido.NumeroPedido,
				ClienteId = pedido.ClienteId,
				NomeCliente = pedido.Cliente.Nome,
				DataPedido = pedido.DataPedido,
				ValorTotal = pedido.ValorTotal,
				Itens = itens
			};
		}

		public async Task AdicionarItensAsync(int pedidoId, List<ItemPedidoDto> itens)
		{
			var pedido = await _db.Pedidos
				.Include(p => p.Itens)
				.FirstOrDefaultAsync(p => p.Id == pedidoId);

			if (pedido == null)
				throw new Exception("Pedido não encontrado.");

			foreach (var itemDto in itens)
			{
				var item = new ItemPedido
				{
					PedidoId = pedido.Id,
					Descricao = itemDto.Descricao,
					Quantidade = itemDto.Quantidade,
					Tamanho = itemDto.Tamanho,
					ValorUnitario = itemDto.ValorUnitario,
					ValorTotal = itemDto.Quantidade * itemDto.ValorUnitario,
					Imagem = Base64ParaByteArray(itemDto.Imagem)
				};

				pedido.Itens.Add(item);
			}

			// Recalcula o valor total do pedido
			pedido.ValorTotal = pedido.Itens.Sum(i => i.ValorTotal);

			await _db.SaveChangesAsync();
		}
	}


}
