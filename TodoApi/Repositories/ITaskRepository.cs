using TodoApi.Models.Dtos;
using TodoApi.Models.Entities;

namespace TodoApi.Repositories;

public interface ITaskRepository
{
	public Task<TodoTask?> GetTask(int id);
	public Task<List<TodoTask>> GetTasks(int userId);
	public Task<TodoTask> AddTask(TodoTask task);
	public Task<TodoTask> EditTask(TodoTask task);
	public Task DeleteTask(int id);
}