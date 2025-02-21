namespace TodoApi.Models.Entities;
public class TodoTask
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required User User { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public bool Completed { get; set; } = false;
}
