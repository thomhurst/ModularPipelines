using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class KubernetesCommandRenderingTests : TestBase
{
    [Test]
    [Arguments(true, " --windows-line-endings")]
    [Arguments(false, " --windows-line-endings=false")]
    [Arguments(null, "")]
    public async Task Edit_Preserves_Explicit_Platform_Boolean(bool? value, string rendered)
    {
        var result = await GetResult(new KubernetesApplyEditLastAppliedOptions
        {
            WindowsLineEndings = value,
        });

        await Assert.That(result.CommandInput).IsEqualTo("kubectl apply edit-last-applied" + rendered);
    }

    [Test]
    [Arguments("--recursive")]
    [Arguments("-R")]
    [Arguments("--recursive=true")]
    [Arguments("--recursive=false")]
    [Arguments("-R=false")]
    [Arguments("--recursive=1")]
    [Arguments("--recursive=0")]
    [Arguments("-R=t")]
    [Arguments("-R=f")]
    public async Task Annotate_Manual_Boolean_Preserves_Following_Option_And_Operand(string flag)
    {
        var result = await GetResult(new KubernetesAnnotateOptions(null)
        {
            Filename = ["manifest.yaml"],
            Arguments = [flag, "owner=team", "--output", "yaml", "--", "another=value"],
            ArgumentsContainToolOptions = true,
            ArgumentsContainOptionTerminator = true,
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo($"kubectl annotate --filename=manifest.yaml {flag} --output yaml owner=team -- another=value");
    }

    [Test]
    public async Task Annotate_Explicit_False_Is_Not_Omitted()
    {
        var result = await GetResult(new KubernetesAnnotateOptions(["owner=team"])
        {
            Recursive = false,
            Filename = ["manifest.yaml"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl annotate --filename=manifest.yaml --recursive=false owner=team");
    }

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
        // Constructor requiredness follows current CLI help; assert the rendered contract.
        var options = JsonSerializer.Deserialize<KubernetesDebugOptions>(
            """{"Pod":"example-pod","Image":"busybox"}""")!;
        var result = await GetResult(options);

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl debug example-pod --image=busybox");
    }

    [Test]
    public async Task Debug_Renders_A_Variadic_Command_Tail()
    {
        var options = JsonSerializer.Deserialize<KubernetesDebugOptions>(
            """{"Pod":"example-pod","CommandArgs":"sh","Args":["-c","echo example"]}""")!;
        var result = await GetResult(options);

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl debug example-pod -- sh -c \"echo example\"");
    }

    [Test]
    public async Task Debug_Filename_Does_Not_Require_A_Pod()
    {
        var options = JsonSerializer.Deserialize<KubernetesDebugOptions>(
            """{"Filename":["pod.yaml"],"Image":"busybox"}""")!;
        var result = await GetResult(options);

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
        var result = await GetResult(new KubernetesLabelOptions(["environment=test"])
        {
            Filename = ["deployment.yaml"],
        });

        await Assert.That(result.CommandInput)
            .IsEqualTo("kubectl label --filename=deployment.yaml environment=test");
    }

    [Test]
    public async Task Label_List_Does_Not_Require_Labels()
    {
        var result = await GetResult(new KubernetesLabelOptions(null!)
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

    [Test]
    public async Task Label_Renders_Resource_Before_All_Labels()
    {
        var result = await GetResult(new KubernetesLabelOptions(["environment=test", "team=build"])
        {
            Type = "pods",
            Name = "example",
        });
        await Assert.That(result.CommandInput).IsEqualTo("kubectl label pods example environment=test team=build");
    }

    [Test]
    public async Task Annotate_Renders_Resource_Before_All_Annotations()
    {
        var result = await GetResult(new KubernetesAnnotateOptions(["owner=build", "note=example"])
        {
            Type = "pods",
            Name = "example",
        });
        await Assert.That(result.CommandInput).IsEqualTo("kubectl annotate pods example owner=build note=example");
    }

    [Test]
    public async Task Label_All_Does_Not_Require_A_Name()
    {
        var result = await GetResult(new KubernetesLabelOptions(["environment=test"])
        {
            Type = "pods",
            All = true,
        });
        await Assert.That(result.CommandInput).IsEqualTo("kubectl label --all pods environment=test");
    }

    [Test]
    public async Task Patch_Distinguishes_Patch_Type_From_Resource_Type()
    {
        var result = await GetResult(new KubernetesPatchOptions
        {
            Type = "merge",
            TypeArgument = "pods",
            Name = "example",
            PatchFile = "patch.json",
        });
        await Assert.That(result.CommandInput).IsEqualTo("kubectl patch pods example --patch-file=patch.json --type=merge");
    }

    [Test]
    public async Task Scale_Renders_Resource_Operands()
    {
        var result = await GetResult(new KubernetesScaleOptions
        {
            Type = "deployments",
            Name = "example",
            Replicas = 3,
        });
        await Assert.That(result.CommandInput).IsEqualTo("kubectl scale --replicas=3 deployments example");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Label_Accepts_Combined_Resource_Or_Kustomize(bool kustomize)
    {
        var result = await GetResult(new KubernetesLabelOptions(["app=test"])
        {
            Type = kustomize ? null : "pod/example",
            Kustomize = kustomize ? "overlay" : null,
        });
        await Assert.That(result.CommandInput).IsEqualTo(kustomize
            ? "kubectl label --kustomize=overlay app=test"
            : "kubectl label pod/example app=test");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Annotate_Accepts_Combined_Resource_Or_Kustomize(bool kustomize)
    {
        var result = await GetResult(new KubernetesAnnotateOptions(["owner=test"])
        {
            Type = kustomize ? null : "pod/example",
            Kustomize = kustomize ? "overlay" : null,
        });
        await Assert.That(result.CommandInput).IsEqualTo(kustomize
            ? "kubectl annotate --kustomize=overlay owner=test"
            : "kubectl annotate pod/example owner=test");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Patch_Accepts_Combined_Resource_Or_Kustomize(bool kustomize)
    {
        var result = await GetResult(new KubernetesPatchOptions
        {
            TypeArgument = kustomize ? null : "pod/example",
            Kustomize = kustomize ? "overlay" : null,
            PatchFile = "patch.json",
        });
        await Assert.That(result.CommandInput).IsEqualTo(kustomize
            ? "kubectl patch --kustomize=overlay --patch-file=patch.json"
            : "kubectl patch pod/example --patch-file=patch.json");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Scale_Accepts_Combined_Resource_Or_Kustomize(bool kustomize)
    {
        var result = await GetResult(new KubernetesScaleOptions
        {
            Type = kustomize ? null : "deployment/example",
            Kustomize = kustomize ? "overlay" : null,
            Replicas = 2,
        });
        await Assert.That(result.CommandInput).IsEqualTo(kustomize
            ? "kubectl scale --kustomize=overlay --replicas=2"
            : "kubectl scale --replicas=2 deployment/example");
    }

    [Test]
    [Arguments("label")]
    [Arguments("annotate")]
    [Arguments("patch")]
    [Arguments("scale")]
    public async Task Name_Without_A_Resource_Source_Is_Rejected(string command)
    {
        CommandLineToolOptions options = command switch
        {
            "label" => new KubernetesLabelOptions(["app=test"]) { Name = "example" },
            "annotate" => new KubernetesAnnotateOptions(["owner=test"]) { Name = "example" },
            "patch" => new KubernetesPatchOptions { Name = "example", PatchFile = "patch.json" },
            "scale" => new KubernetesScaleOptions { Name = "example", Replicas = 2 },
            _ => throw new ArgumentOutOfRangeException(nameof(command)),
        };
        await Assert.ThrowsAsync<CommandOptionsValidationException>(() => GetResult(options));
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommandContext>();
        return await command.ExecuteCommandLineToolAsync(options, new CommandExecutionOptions { InternalDryRun = true });
    }
}
