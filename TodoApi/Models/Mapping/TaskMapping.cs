using TodoApi.Models.Entities;
using TodoApi.Models.Dtos;

namespace TodoApi.Models.Mapping;

public static class TaskMapping
{
    public static TodoTask ToEntity(this AddTaskDto task, User user)
	{
		return new TodoTask
		{
			Name = task.Name,
			Description = task.Description,
			User = user
		};
	}
	public static TodoTask ToEntity(this EditTaskDto task, TodoTask oldTask)
	{
		oldTask.Name = task.Name ?? oldTask.Name;
		oldTask.Description = task.Description ?? oldTask.Description;
		oldTask.Completed = task.Completed ?? oldTask.Completed;
		return oldTask;
	}
}