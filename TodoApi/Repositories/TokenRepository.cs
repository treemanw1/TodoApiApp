using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Models.Entities;
using TodoApi.Data;
using TodoApi.Models.Dtos;
using TodoApi.Models.Mapping;

namespace TodoApi.Repositories;

public class TokenRepository : ITokenRepository
{
	private readonly TodoContext _dbContext;
	private readonly JwtSettings _jwtSettings;
	public TokenRepository(TodoContext dbContext, IOptions<JwtSettings> jwtSettings)
	{
		_dbContext = dbContext;
		_jwtSettings = jwtSettings.Value;
	}
	public async Task<Token?> GetToken(int id)
	{
		return await _dbContext.Tokens.FindAsync(id);
	}
	public async Task<Token?> GetToken(string token)
	{
		return await _dbContext.Tokens.FirstOrDefaultAsync(t => t.Value == token);
	}
	public async Task<Token> AddToken(Token token)
	{
		token.Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.MagicTokenExpirationInMinutes);
		_dbContext.Add(token);
		await _dbContext.SaveChangesAsync();
		return token;
	}
	public async Task<Token> UpdateToken(Token token)
	{
		_dbContext.Update(token);
		await _dbContext.SaveChangesAsync();
		return token;
	}
}