namespace TecnoFix.Src.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string destino, string asunto, string cuerpoHtml);
}