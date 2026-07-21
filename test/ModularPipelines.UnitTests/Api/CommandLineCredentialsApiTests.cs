using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Api;

public class CommandLineCredentialsApiTests
{
    [Test]
    public async Task CommandExecutionOptions_ExposeFrameworkOwnedCredentials()
    {
        var property = typeof(CommandExecutionOptions).GetProperty(nameof(CommandExecutionOptions.CommandLineCredentials));

        await Assert.That(property).IsNotNull();
        await Assert.That(property!.PropertyType).IsEqualTo(typeof(CommandLineCredentials));
        await Assert.That(property.PropertyType.Assembly).IsEqualTo(typeof(CommandExecutionOptions).Assembly);
    }
}
