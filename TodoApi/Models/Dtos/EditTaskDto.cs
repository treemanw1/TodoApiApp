namespace TodoApi.Models.Dtos;

public record EditTaskDto(
	int Id,
	string? Name,
	string? Description,
	bool? Completed
);