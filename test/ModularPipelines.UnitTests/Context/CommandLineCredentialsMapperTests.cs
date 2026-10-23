using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Context;

public class CommandLineCredentialsMapperTests
{
    [Test]
    public async Task Apply_MapsEveryProperty()
    {
        if (!OperatingSystem.IsWindows())
        {
            Skip.Test("Windows only test");
            return;
        }

        var credentials = new CommandLineCredentials
        {
            Domain = "domain",
            UserName = "user",
            Password = "password",
            LoadUserProfile = true
        };
        var startInfo = new ProcessStartInfo();

        CommandLineCredentialsMapper.Apply(credentials, startInfo);

        await Assert.That(startInfo.Domain).IsEqualTo(credentials.Domain);
        await Assert.That(startInfo.UserName).IsEqualTo(credentials.UserName);
        await Assert.That(startInfo.PasswordInClearText).IsEqualTo(credentials.Password);
        await Assert.That(startInfo.LoadUserProfile).IsEqualTo(credentials.LoadUserProfile);
    }

}
