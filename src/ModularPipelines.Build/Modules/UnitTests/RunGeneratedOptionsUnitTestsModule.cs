using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.Modules.UnitTests;

[DependsOn<RunCoreUnitTestsModule>]
public abstract class RunGeneratedOptionsUnitTestsModule(
    IOptions<PipelineSettings> pipelineSettings)
    : RunUnitTestModule(pipelineSettings)
{
    private const string TypeNamePrefix = "Run";
    private const string TypeNameSuffix = "UnitTestsModule";

    protected override string TestProjectFileName =>
        $"ModularPipelines.{GetIntegrationName(GetType())}.UnitTests.csproj";

    private static string GetIntegrationName(Type moduleType)
    {
        var name = moduleType.Name;
        if (!name.StartsWith(TypeNamePrefix, StringComparison.Ordinal)
            || !name.EndsWith(TypeNameSuffix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Generated options test module '{name}' must be named '{TypeNamePrefix}<Integration>{TypeNameSuffix}'.");
        }

        return name[TypeNamePrefix.Length..^TypeNameSuffix.Length];
    }
}

public static class GeneratedOptionsUnitTestProjects
{
    public static Type[] ModuleTypes { get; } =
    [
        .. typeof(RunGeneratedOptionsUnitTestsModule).Assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsClass: true }
                && type.IsAssignableTo(typeof(RunGeneratedOptionsUnitTestsModule)))
            .OrderBy(type => type.Name, StringComparer.Ordinal),
    ];
}

public sealed class RunAmazonWebServicesUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunBuildahUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunChocolateyUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunDotNetUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunFluxUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunFlywayUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunGitUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunGoUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunGoogleUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunGrypeUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunHelmUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunHomebrewUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunKindUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunKubernetesUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunMinikubeUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunNewmanUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunPackerUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunPodmanUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunPulumiUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunPythonUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunRustUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunSkopeoUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunSyftUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunTerraformUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunVaultUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunWinGetUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunYarnUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);

public sealed class RunYqUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunGeneratedOptionsUnitTestsModule(pipelineSettings);
