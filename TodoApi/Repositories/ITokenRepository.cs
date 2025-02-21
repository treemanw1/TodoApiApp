using TodoApi.Models.Dtos;
using TodoApi.Models.Entities;

namespace TodoApi.Repositories;

public interface ITokenRepository
{
	public Task<Token?> GetToken(int id);
	public Task<Token?> GetToken(string token);
	public Task<Token> AddToken(Token token);
	public Task<Token> UpdateToken(Token task);
	// public Task<Task> DeleteTask(int id);
}