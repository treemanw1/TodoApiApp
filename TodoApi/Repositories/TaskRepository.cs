using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entities;
using TodoApi.Data;
using TodoApi.Models.Dtos;
using TodoApi.Models.Mapping;

namespace TodoApi.Repositories;

public class TaskRepository : ITaskRepository
{
	private readonly TodoContext _dbContext;
	public TaskRepository(TodoContext dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task<TodoTask?> GetTask(int id)
	{
		return await _dbContext.Tasks.FindAsync(id);
	}
	public async Task<List<TodoTask>> GetTasks(int userId)
	{
		return await _dbContext.Tasks.Where(t => t.User.Id == userId).ToListAsync();
	}
	public async Task<TodoTask> AddTask(TodoTask task)
	{
		_dbContext.Add(task);
		await _dbContext.SaveChangesAsync();
		return task;
	}
	public async Task<TodoTask> EditTask(TodoTask task)
	{
		_dbContext.Update(task);
		await _dbContext.SaveChangesAsync();
		return task;
	}
	public async Task DeleteTask(int id)
	{
		var task = await _dbContext.Tasks.FindAsync(id);
		if (task != null)
		{
			_dbContext.Remove(task);
			await _dbContext.SaveChangesAsync();
		}
	}
}