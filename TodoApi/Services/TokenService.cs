using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
public class TokenService : ITokenService
{
	private readonly JwtSettings _configuration;
    public TokenService(IOptions<JwtSettings> configuration)
	{
		_configuration = configuration.Value;
	}

	public string GenerateMagicToken(int userId)
	{
		var claims = new List<Claim>
		{
			new("userId", userId.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};
		var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.MagicTokenExpirationInMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey)),
                SecurityAlgorithms.HmacSha256)
        );
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
	public string GenerateAccessToken(int userId)
	{
		var claims = new List<Claim>
		{
			new("userId", userId.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};
		var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationInMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey)),
                SecurityAlgorithms.HmacSha256)
        );
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
	public ClaimsPrincipal ValidateJwtToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_configuration.SecretKey);
		try
		{
			var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _configuration.Issuer,
				ValidAudience = _configuration.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ClockSkew = TimeSpan.FromMinutes(2)
			}, out SecurityToken validatedToken);

			return claimsPrincipal;
		}
		catch
		{
			return null;
		}
	}
}
