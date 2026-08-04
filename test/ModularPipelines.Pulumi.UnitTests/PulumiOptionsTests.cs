using ModularPipelines.Context;
using ModularPipelines.Pulumi.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Pulumi.UnitTests;

public class PulumiOptionsTests : TestBase
{
    [Test]
    public async Task Hoists_EqualsSeparated_Option_Before_Provider_Parameters()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new PulumiPackageInfoOptions("aws")
        {
            ProviderParameter = ["param"],
            Arguments = ["--color=never"],
            ArgumentsContainToolOptions = true,
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("pulumi package info --color=never aws -- param");
    }
}
