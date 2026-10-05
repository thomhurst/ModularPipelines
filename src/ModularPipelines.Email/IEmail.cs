namespace ModularPipelines.Email;

public interface IEmail
{
    Task<string> SendAsync(EmailSendOptions options, CancellationToken cancellationToken = default);
}