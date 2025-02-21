namespace TodoApi.Models.Entities;
public class Token
{
	public int Id { get; set; }
	public required User User { get; set; }
	public required string Value { get; set; }
	public DateTime Expiration { get; set; }
	public bool Used { get; set; } = false;
}
