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
				NumeroPedido = numeroPedido,
				Status = "Não iniciado",
				DataEntrega = null
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

		public async Task<PedidoRespostaDto> GetByIdAsync(int id)
		{
			var pedido = await _db.Pedidos
			.Include(p => p.Itens)
			.Include(p => p.Cliente)  // Inclui o cliente
			.FirstOrDefaultAsync(p => p.Id == id);

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
				Status = pedido.Status,
				DataEntrega = pedido.DataEntrega,
				Itens = itens
			};
		}



		public async Task AdicionarItensAsync(int pedidoId, ItemPedidoDto itemDto)
		{
			var pedido = await _db.Pedidos
				.Include(p => p.Itens)
				.FirstOrDefaultAsync(p => p.Id == pedidoId);

			if (pedido == null)
				throw new Exception("Pedido não encontrado.");

			var item = new ItemPedido
			{
				PedidoId = pedido.Id,
				Descricao = itemDto.Descricao,
				Quantidade = itemDto.Quantidade,
				Tamanho = itemDto.Tamanho,
				ValorUnitario = itemDto.ValorUnitario,
				ValorTotal = itemDto.Quantidade * itemDto.ValorUnitario,
				Imagem = Base64ParaByteArray(itemDto.Imagem),

				// 🔹 Campos opcionais: se não vier, default para "Não" ou "Não iniciado"
				Prioridade = itemDto.Prioridade ?? "Não",
				TemPintura = itemDto.TemPintura ?? "Não",
				StatusPintura = itemDto.StatusPintura ?? "Não iniciado",
				TemBordado = itemDto.TemBordado ?? "Não",
				StatusBordado = itemDto.StatusBordado ?? "Não iniciado",
				TemDtf = itemDto.TemDtf ?? "Não",
				StatusDtf = itemDto.StatusDtf ?? "Não iniciado",
				TemSilk = itemDto.TemSilk ?? "Não",
				StatusSilk = itemDto.StatusSilk ?? "Não iniciado",

				// 🔹 Etapas obrigatórias: sempre marcadas como "Sim" e status inicial "Não iniciado"
				TemCorte = "Sim",
				StatusCorte = "Não iniciado",
				TemCostura = "Sim",
				StatusCostura = "Não iniciado",
				TemDobragem = "Sim",
				StatusDobragem = "Não iniciado",
				TemConferencia = "Sim",
				StatusConferencia = "Não iniciado"
			};

			pedido.Itens.Add(item);

			// Recalcula o valor total do pedido
			pedido.ValorTotal = pedido.Itens.Sum(i => i.ValorTotal);

			await _db.SaveChangesAsync();
		}




		public async Task AtualizarDataEntregaAsync(int pedidoId, DateTime novaDataEntrega)
		{
			var pedido = await _db.Pedidos.FindAsync(pedidoId);
			if (pedido == null)
			{
				throw new KeyNotFoundException($"Pedido {pedidoId} não encontrado");
			}

			pedido.DataEntrega = novaDataEntrega;

			_db.Pedidos.Update(pedido);
			await _db.SaveChangesAsync();
		}

		public async Task<List<PedidoRespostaDto>> GetAsync()
		{
			// Busca todos os pedidos incluindo Cliente e Itens
			var pedidos = await _db.Pedidos
				.Include(p => p.Itens)
				.Include(p => p.Cliente)
				.ToListAsync();

			// Projeta para PedidoRespostaDto
			var resultado = pedidos.Select(pedido => new PedidoRespostaDto
			{
				Id = pedido.Id,
				NumeroPedido = pedido.NumeroPedido,
				ClienteId = pedido.ClienteId,
				NomeCliente = pedido.Cliente.Nome,
				DataPedido = pedido.DataPedido,
				ValorTotal = pedido.ValorTotal,
				Status = pedido.Status,
				DataEntrega = pedido.DataEntrega,
				Itens = pedido.Itens.Select(i => new ItemPedidoDto
				{
					Descricao = i.Descricao,
					Quantidade = i.Quantidade,
					Tamanho = i.Tamanho,
					ValorUnitario = i.ValorUnitario,
					Imagem = i.Imagem != null ? Convert.ToBase64String(i.Imagem) : null,

					// Campos opcionais
					Prioridade = i.Prioridade,
					TemPintura = i.TemPintura,
					StatusPintura = i.StatusPintura,
					TemBordado = i.TemBordado,
					StatusBordado = i.StatusBordado,
					TemDtf = i.TemDtf,
					StatusDtf = i.StatusDtf,
					TemSilk = i.TemSilk,
					StatusSilk = i.StatusSilk,

					// Etapas obrigatórias
					TemCorte = i.TemCorte,
					StatusCorte = i.StatusCorte,
					TemCostura = i.TemCostura,
					StatusCostura = i.StatusCostura,
					TemDobragem = i.TemDobragem,
					StatusDobragem = i.StatusDobragem,
					TemConferencia = i.TemConferencia,
					StatusConferencia = i.StatusConferencia
				}).ToList()
			}).ToList();

			return resultado;
		}

	}


}
