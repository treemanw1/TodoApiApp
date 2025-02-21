using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entities;

namespace TodoApi.Data;

public class TodoContext: DbContext
{
	public TodoContext(DbContextOptions<TodoContext> options): base(options) { }

	public DbSet<User> Users{ get; set; }
	public DbSet<TodoTask> Tasks{ get; set; }
	public DbSet<Token> Tokens{ get; set; }
}