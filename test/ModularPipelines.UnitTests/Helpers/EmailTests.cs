using System.Net;
using MailKit.Security;
using MimeKit;
using ModularPipelines.Email;
using ModularPipelines.Email.Options;

namespace ModularPipelines.UnitTests.Helpers;

public class EmailTests : TestBase
{
    private readonly string _emailAddress = $"{Guid.NewGuid():N}@_____.com";
    
    [Test, Ignore("Need an SMTP server")]
    public async Task Can_Send_Email()
    {
        var email = await GetService<IEmail>();

        var response = await email.SendAsync(
            new EmailSendOptions(
                From: _emailAddress,
                To: new[] {_emailAddress},
                Subject: "Email Test",
                Body: new TextPart { Text = "This is an email test." },
                SmtpServerHost: "_____.com"
            )
            {
                SecureSocketOptions = SecureSocketOptions.Auto
            }
        );
        
        Assert.That(response, Is.EqualTo(""));
    }
}