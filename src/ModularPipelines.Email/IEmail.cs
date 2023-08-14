using ModularPipelines.Email.Options;

namespace ModularPipelines.Email;

public interface IEmail
{
    Task SendAsync(EmailSendOptions options, CancellationToken cancellationToken = default);
}
