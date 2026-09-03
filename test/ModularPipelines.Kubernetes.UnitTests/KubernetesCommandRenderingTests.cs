using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class KubernetesCommandRenderingTests : TestBase
{
    [Test]
    public async Task Apply_Validate_Renders_Selected_Mode()
    {
        var result = await GetResult(new KubernetesApplyOptions
        {
            Filename = ["manifest.yaml"],
            Validate = "warn",
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl apply --filename=manifest.yaml --validate=warn");
    }

    [Test]
    public async Task Kustomize_Create_Joins_Scalar_Map_Entries()
    {
        var result = await GetResult(new KustomizeCreateOptions
        {
            Annotations =
            [
                "owners:alice",
                "tier:backend",
            ],
            Labels =
            [
                "app:web",
                "environment:test",
            ],
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "kustomize create --annotations=owners:alice,tier:backend "
            + "--labels=app:web,environment:test");
    }

    [Test]
    public async Task Auth_CanI_List_Does_Not_Require_A_Verb()
    {
        var result = await GetResult(new KubernetesAuthCanIOptions(null!) { List = true });

        await Assert.That(result.CommandInput).IsEqualTo("kubectl auth can-i --list");
    }

    [Test]
    public async Task Debug_Does_Not_Require_A_Command()
    {
        var result = await GetResult(new KubernetesDebugOptions("example-pod", null!)
        {
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
    public async Task Debug_Filename_Does_Not_Require_A_Pod()
    {
        var result = await GetResult(new KubernetesDebugOptions(null!, null!)
        {
            Filename = ["pod.yaml"],
            Image = "busybox",
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl debug --filename=pod.yaml --image=busybox");
    }

    [Test]
    public async Task Exec_Filename_Does_Not_Require_A_Pod()
    {
        var result = await GetResult(new KubernetesExecOptions(null!, "env")
        {
            Filename = ["pod.yaml"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl exec --filename=pod.yaml -- env");
    }

    [Test]
    public async Task Label_File_With_One_Label_Does_Not_Require_Another_Label()
    {
        var result = await GetResult(new KubernetesLabelOptions(["environment=test"], null!)
        {
            Filename = ["deployment.yaml"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl label --filename=deployment.yaml environment=test");
    }

    [Test]
    public async Task Label_List_Does_Not_Require_Labels()
    {
        var result = await GetResult(new KubernetesLabelOptions(null!, null!)
        {
            Filename = ["deployment.yaml"],
            List = true,
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl label --filename=deployment.yaml --list");
    }

    [Test]
    public async Task Taint_All_Nodes_Does_Not_Require_A_Name()
    {
        var result = await GetResult(new KubernetesTaintOptions(
            "nodes",
            null!,
            ["example=value:NoSchedule"])
        {
            All = true,
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
