using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entities;
using TodoApi.Data;
using TodoApi.Models.Dtos;

namespace TodoApi.Repositories;

public class UserRepository : IUserRepository
{
	private readonly TodoContext _dbContext;
	public UserRepository(TodoContext dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task<User?> GetUser(int id)
	{
		return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
	}
	public async Task<User?> GetUser(string email)
	{
		return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
	}
	public async Task<User> AddUser(User user)
	{
		_dbContext.Add(user);
		await _dbContext.SaveChangesAsync();
		return user;
	}
}