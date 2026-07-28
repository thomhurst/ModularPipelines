using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Enums;

namespace ModularPipelines.Build.Modules.UnitTests;

[ExecutionHint(ExecutionType.CpuIntensive)]
public sealed class RunGeneratedOptionsUnitTestsModule<TProject>(
    IOptions<PipelineSettings> pipelineSettings)
    : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName =>
        $"ModularPipelines.{typeof(TProject).Name}.UnitTests.csproj";
}

public static class GeneratedOptionsUnitTestProjects
{
    public static Type[] ModuleTypes { get; } = GetModuleTypes();

    public sealed class AmazonWebServices;

    public sealed class Buildah;

    public sealed class Chocolatey;

    public sealed class DotNet;

    public sealed class Flux;

    public sealed class Flyway;

    public sealed class Git;

    public sealed class Go;

    public sealed class Google;

    public sealed class Grype;

    public sealed class Helm;

    public sealed class Homebrew;

    public sealed class Kind;

    public sealed class Kubernetes;

    public sealed class Minikube;

    public sealed class Newman;

    public sealed class Packer;

    public sealed class Podman;

    public sealed class Pulumi;

    public sealed class Python;

    public sealed class Rust;

    public sealed class Skopeo;

    public sealed class Syft;

    public sealed class Terraform;

    public sealed class Vault;

    public sealed class WinGet;

    public sealed class Yarn;

    public sealed class Yq;

    private static Type[] GetModuleTypes()
    {
        Type[] projectTypes =
        [
            typeof(AmazonWebServices),
            typeof(Buildah),
            typeof(Chocolatey),
            typeof(DotNet),
            typeof(Flux),
            typeof(Flyway),
            typeof(Git),
            typeof(Go),
            typeof(Google),
            typeof(Grype),
            typeof(Helm),
            typeof(Homebrew),
            typeof(Kind),
            typeof(Kubernetes),
            typeof(Minikube),
            typeof(Newman),
            typeof(Packer),
            typeof(Podman),
            typeof(Pulumi),
            typeof(Python),
            typeof(Rust),
            typeof(Skopeo),
            typeof(Syft),
            typeof(Terraform),
            typeof(Vault),
            typeof(WinGet),
            typeof(Yarn),
            typeof(Yq),
        ];

        return
        [
            .. projectTypes.Select(type =>
                typeof(RunGeneratedOptionsUnitTestsModule<>).MakeGenericType(type)),
        ];
    }
}
