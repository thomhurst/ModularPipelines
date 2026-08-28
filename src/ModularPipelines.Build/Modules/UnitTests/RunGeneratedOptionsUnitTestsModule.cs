using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Enums;

namespace ModularPipelines.Build.Modules.UnitTests;

[ExecutionHint(ExecutionHint.CpuBound)]
[DependsOn<RunCoreUnitTestsModule>]
public abstract class RunGeneratedOptionsUnitTestsModule(
    IOptions<PipelineSettings> pipelineSettings)
    : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName =>
        $"ModularPipelines.{GetType().Name}.UnitTests.csproj";
}

public static class GeneratedOptionsUnitTestProjects
{
    public static Type[] ModuleTypes { get; } =
    [
        .. typeof(GeneratedOptionsUnitTestProjects)
            .GetNestedTypes()
            .Where(type => type.IsAssignableTo(typeof(RunGeneratedOptionsUnitTestsModule)))
            .OrderBy(type => type.Name),
    ];

    public sealed class AmazonWebServices(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Buildah(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Chocolatey(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class DotNet(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Flux(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Flyway(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Git(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Go(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Google(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Grype(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Helm(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Homebrew(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Kind(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Kubernetes(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Minikube(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Newman(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Packer(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Podman(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Pulumi(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Python(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Rust(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Skopeo(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Syft(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Terraform(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Vault(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class WinGet(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Yarn(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

    public sealed class Yq(IOptions<PipelineSettings> pipelineSettings)
        : RunGeneratedOptionsUnitTestsModule(pipelineSettings);
}
