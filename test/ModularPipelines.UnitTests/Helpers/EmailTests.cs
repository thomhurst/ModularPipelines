using System.Net;
using MailKit.Security;
using MimeKit;
using ModularPipelines.Email;
using ModularPipelines.Email.Options;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;
using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.Helpers;

public class EmailTests : TestBase
{
    private const string EmailAddress = "modularpipelinestest@gmail.com";

    public async Task Can_Send_Email()
    {
        var email = await GetService<IEmail>();

        var emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

        if (string.IsNullOrEmpty(emailPassword))
        {
            throw new SkipTestException("No email password");
        }

        var response = await email.SendAsync(
            new EmailSendOptions(
                From: EmailAddress,
                To: new[] { EmailAddress },
                Subject: "Email Test",
                Body: new TextPart { Text = "This is an email test." },
                SmtpServerHost: "smtp-relay.brevo.com"
            )
            {
                Port = 587,
                Credentials = new NetworkCredential(EmailAddress, emailPassword),
                SecureSocketOptions = SecureSocketOptions.StartTls,
                ClientConfigurator = client =>
                {
                },
            }
        );
        await Assert.That(response).Does.Contain("queued");
    }
}
