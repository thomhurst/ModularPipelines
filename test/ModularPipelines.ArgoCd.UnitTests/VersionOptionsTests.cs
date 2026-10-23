using ModularPipelines.ArgoCd.Enums;
using ModularPipelines.ArgoCd.Options;
using ModularPipelines.Context;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.ArgoCd.UnitTests;

public class VersionOptionsTests : TestBase
{
    [Test]
    public async Task Client_Version_Renders_Without_Server_Connection()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new ArgoCdVersionOptions
        {
            Client = true,
            Output = ArgoCdVersionOutput.Json,
        };

        await Assert.That(RenderCommand(builder.Build(options))).IsEqualTo("argocd version --client --output=json");
    }
}
