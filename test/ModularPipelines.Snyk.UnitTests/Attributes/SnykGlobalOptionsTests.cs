using ModularPipelines.Context;
using ModularPipelines.Snyk.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Snyk.UnitTests.Attributes;

public class SnykGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Debug_Is_Inherited_And_Renders_Once_Before_Command()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new SnykPolicyOptions { Debug = true, PathToPolicyFile = ".snyk" };
        await Assert.That(builder.Build(options).ToString()).IsEqualTo("snyk -d policy .snyk");
        await Assert.That(typeof(SnykPolicyOptions).GetProperty(nameof(SnykOptions.Debug))!.DeclaringType)
            .IsEqualTo(typeof(SnykOptions));
    }
}
