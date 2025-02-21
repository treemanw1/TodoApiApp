using TodoApi.Models.Entities;

namespace TodoApi.Repositories;

public interface IUserRepository
{
	public Task<User?> GetUser(int id);
	public Task<User?> GetUser(string email);
	public Task<User> AddUser(User user);
	// public Task<Task> UpdateTask(Task task);
	// public Task<Task> DeleteTask(int id);
}