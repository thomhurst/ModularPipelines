using MailKit.Security;
using MimeKit;
using ModularPipelines.Email;
using ModularPipelines.Email.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Email.UnitTests.Helpers;

public class EmailTests : TestBase
{
    private const string EmailAddress = "test@example.com";

    [Test]
    public async Task Can_Send_Email()
    {
        var email = await GetService<IEmail>();
        await using var smtpServer = LocalSmtpServer.Start();

        var response = await email.SendAsync(
            new EmailSendOptions(
                From: EmailAddress,
                To: [EmailAddress],
                Subject: "Email Test",
                Body: new TextPart { Text = "This is an email test." },
                SmtpServerHost: "127.0.0.1"
            )
            {
                Port = smtpServer.Port,
                SecureSocketOptions = SecureSocketOptions.None,
            }
        );

        var message = await smtpServer.Message;

        await Assert.That(response).Contains("queued");
        await Assert.That(message).Contains("Subject: Email Test");
        await Assert.That(message).Contains("This is an email test.");
    }
}
