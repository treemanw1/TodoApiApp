using System.Security.Claims;

public interface IEmailService
{
    public bool ValidEmail(string email);
	public Task SendMagicLinkEmail(string email, string link);
}