using System.Security.Claims;

public interface ITokenService
{
	public string GenerateMagicToken(int userId);
	public string GenerateAccessToken(int userId);
	public ClaimsPrincipal ValidateJwtToken(string token);
}