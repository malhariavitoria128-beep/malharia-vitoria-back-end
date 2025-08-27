using malharia_back_end.Data;
using malharia_back_end.Dtos.ClientesDtos;
using malharia_back_end.Models;
using malharia_back_end.Services.Interfaces;
using Serilog;
using Microsoft.EntityFrameworkCore;


namespace malharia_back_end.Services.Services
{
	public class ClienteService : IClienteService
	{
		private readonly Context _db;

		public ClienteService(Context db)
		{
			_db = db;
		}

		public async Task RegisterAsync(ClienteDto dto)
		{
			try
			{
				// Normaliza strings vazias para null
				var cpf = string.IsNullOrWhiteSpace(dto.CPF) ? null : dto.CPF;
				var cnpj = string.IsNullOrWhiteSpace(dto.CNPJ) ? null : dto.CNPJ;
				var email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email;
				var telefone = string.IsNullOrWhiteSpace(dto.Telefone) ? null : dto.Telefone;
				var cep = string.IsNullOrWhiteSpace(dto.Cep) ? null : dto.Cep;
				var endereco = string.IsNullOrWhiteSpace(dto.Endereco) ? null : dto.Endereco;

				// Nome obrigatório e único
				if (await _db.Clientes.AnyAsync(c => c.Nome == dto.Nome))
					throw new Exception("Nome já cadastrado.");

				// CPF único se preenchido
				if (cpf != null &&
					await _db.Clientes.AnyAsync(c => c.CPF == cpf))
					throw new Exception("CPF já cadastrado.");

				// CNPJ único se preenchido
				if (cnpj != null &&
					await _db.Clientes.AnyAsync(c => c.CNPJ == cnpj))
					throw new Exception("CNPJ já cadastrado.");

				var cliente = new Cliente
				{
					Nome = dto.Nome,
					CPF = cpf,
					CNPJ = cnpj,
					Email = email,
					Telefone = telefone,
					CEP = cep,
					Endereco = endereco
				};

				_db.Clientes.Add(cliente);
				await _db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao registrar cliente {Nome}", dto.Nome);
				throw;
			}
		}


		public async Task<List<BuscaClienteDto>> GetAllAsync()
		{
			return await _db.Clientes
				.Select(c => new BuscaClienteDto
				{
					Nome = c.Nome,
					CPF = c.CPF,
					CNPJ = c.CNPJ,
					Email = c.Email,
					Telefone = c.Telefone,
					Cep = c.CEP,
					Endereco = c.Endereco,
					Id = c.Id
				})
				.ToListAsync();
		}

		public async Task<BuscaClienteDto?> GetByIdAsync(int id)
		{
			return await _db.Clientes
				.Where(c => c.Id == id)
				.Select(c => new BuscaClienteDto
				{
					Nome = c.Nome,
					CPF = c.CPF,
					CNPJ = c.CNPJ,
					Email = c.Email,
					Telefone = c.Telefone,
					Cep = c.CEP,
					Endereco = c.Endereco,
					Id = c.Id
				})
				.FirstOrDefaultAsync();
		}

		public async Task UpdateAsync(int id, ClienteDto dto)
		{
			try
			{
				// Normaliza strings vazias para null
				var cpf = string.IsNullOrWhiteSpace(dto.CPF) ? null : dto.CPF;
				var cnpj = string.IsNullOrWhiteSpace(dto.CNPJ) ? null : dto.CNPJ;
				var email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email;
				var telefone = string.IsNullOrWhiteSpace(dto.Telefone) ? null : dto.Telefone;
				var cep = string.IsNullOrWhiteSpace(dto.Cep) ? null : dto.Cep;
				var endereco = string.IsNullOrWhiteSpace(dto.Endereco) ? null : dto.Endereco;

				// Buscar o cliente existente
				var cliente = await _db.Clientes.FindAsync(id);

				if (cliente == null)
					throw new Exception($"Cliente com ID {id} não encontrado.");

				// Nome obrigatório e único (exceto o próprio)
				if (await _db.Clientes.AnyAsync(c => c.Nome == dto.Nome && c.Id != id))
					throw new Exception("Nome já cadastrado para outro cliente.");

				// CPF único se preenchido (exceto o próprio)
				if (cpf != null &&
					await _db.Clientes.AnyAsync(c => c.CPF == cpf && c.Id != id))
					throw new Exception("CPF já cadastrado para outro cliente.");

				// CNPJ único se preenchido (exceto o próprio)
				if (cnpj != null &&
					await _db.Clientes.AnyAsync(c => c.CNPJ == cnpj && c.Id != id))
					throw new Exception("CNPJ já cadastrado para outro cliente.");

				// Atualizar os dados do cliente
				cliente.Nome = dto.Nome;
				cliente.CPF = cpf;
				cliente.CNPJ = cnpj;
				cliente.Email = email;
				cliente.Telefone = telefone;
				cliente.CEP = cep;
				cliente.Endereco = endereco;
			
				_db.Clientes.Update(cliente);
				await _db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao atualizar cliente {Id}", id);
				throw;
			}
		}

		public async Task DeleteClienteAsync(int clienteId)
		{
			try
			{
				var cliente = await _db.Clientes.FindAsync(clienteId)
						   ?? throw new Exception("Cliente não encontrado.");

				_db.Clientes.Remove(cliente);
				await _db.SaveChangesAsync();

				Log.Information("Cliente ID {ClienteId} removido com sucesso", clienteId);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erro ao remover usuário ID {UserId}", clienteId);
				throw;
			}
		}

	}
}
