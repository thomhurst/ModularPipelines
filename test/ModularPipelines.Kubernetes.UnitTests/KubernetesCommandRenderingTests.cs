using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class KubernetesCommandRenderingTests : TestBase
{
    [Test]
    public async Task Auth_CanI_List_Does_Not_Require_A_Verb()
    {
        var result = await GetResult(new KubernetesAuthCanIOptions { List = true });

        await Assert.That(result.CommandInput).IsEqualTo("kubectl auth can-i --list");
    }

    [Test]
    public async Task Debug_Does_Not_Require_A_Command()
    {
        var result = await GetResult(new KubernetesDebugOptions
        {
            Pod = "example-pod",
            Image = "busybox",
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl debug example-pod --image=busybox");
    }

    [Test]
    public async Task Debug_Renders_A_Variadic_Command_Tail()
    {
        var result = await GetResult(new KubernetesDebugOptions("example-pod", "sh")
        {
            Args = ["-c", "echo example"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl debug example-pod -- sh -c \"echo example\"");
    }

    [Test]
    public async Task Label_File_With_One_Label_Does_Not_Require_Another_Label()
    {
        var result = await GetResult(new KubernetesLabelOptions
        {
            Filename = ["deployment.yaml"],
            Key_1Val_1 = ["environment=test"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl label --filename=deployment.yaml environment=test");
    }

    [Test]
    public async Task Taint_All_Nodes_Does_Not_Require_A_Name()
    {
        var result = await GetResult(new KubernetesTaintOptions
        {
            Node = "nodes",
            All = true,
            Taints = ["example=value:NoSchedule"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl taint nodes example=value:NoSchedule --all");
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommandContext>();
        return await command.ExecuteCommandLineToolAsync(options, new CommandExecutionOptions { InternalDryRun = true });
    }
}
