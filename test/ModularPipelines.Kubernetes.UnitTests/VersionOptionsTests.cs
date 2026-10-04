using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Enums;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class VersionOptionsTests : TestBase
{
    [Test]
    public async Task Client_Version_Renders_Json_Output()
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(new KubernetesVersionOptions
        {
            Client = true,
            Output = KubernetesVersionOutput.Json,
        }, new CommandExecutionOptions { InternalDryRun = true });

        await Assert.That(result.CommandInput).IsEqualTo("kubectl version --client --output=json");
    }
}
