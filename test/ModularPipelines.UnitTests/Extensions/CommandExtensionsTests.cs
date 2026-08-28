using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Extensions;

public class CommandExtensionsTests
{
    private sealed record DerivedOptions : CommandLineToolOptions;

    [Test]
    public async Task WithArguments_AddsToExisting()
    {
        var commandLineOptions = new DerivedOptions
        {
            Arguments = ["arg1", "arg2"],
        }
            .WithArguments(["arg3", "arg4", "arg5"]);

        using (Assert.Multiple())
        {
            await Assert.That(commandLineOptions).IsTypeOf<DerivedOptions>();
            await Assert.That(commandLineOptions.Arguments!).IsEquivalentTo(["arg1", "arg2", "arg3", "arg4", "arg5"]);
        }
    }
}
