using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Context;

public class CommandLineCredentialsMapperTests
{
    [Test]
    public async Task ToCliWrapCredentials_MapsEveryProperty()
    {
        var credentials = new CommandLineCredentials
        {
            Domain = "domain",
            UserName = "user",
            Password = "password",
            LoadUserProfile = true
        };

        var result = credentials.ToCliWrapCredentials();

        await Assert.That(result.Domain).IsEqualTo(credentials.Domain);
        await Assert.That(result.UserName).IsEqualTo(credentials.UserName);
        await Assert.That(result.Password).IsEqualTo(credentials.Password);
        await Assert.That(result.LoadUserProfile).IsEqualTo(credentials.LoadUserProfile);
    }
}
