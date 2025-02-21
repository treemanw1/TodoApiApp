using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
	private readonly EmailSettings _emailSettings = emailSettings.Value;

	public bool ValidEmail(string email)
	{
		try
        {
            var message = new MailboxAddress("", email);
        }
        catch (FormatException)
        {
			return false;
        }
		return true;
	}
	public async Task SendMagicLinkEmail(string email, string token)
	{
		var message = new MimeMessage();
		message.From.Add(new MailboxAddress("", _emailSettings.SenderEmail));
		message.To.Add(new MailboxAddress("", email));
		message.Subject = "Magic Link";
		message.Body = new TextPart("plain")
		{
			Text = token
		};
		using (var client = new SmtpClient())
		{
			await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
			await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
			await client.SendAsync(message);
			await client.DisconnectAsync(true);
		}
	}
}