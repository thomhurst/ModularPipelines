using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "build-service", "build", "create")]
public record AzSpringBuildServiceBuildCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--apms")]
    public string? Apms { get; set; }

    [CommandSwitch("--artifact-path")]
    public string? ArtifactPath { get; set; }

    [CommandSwitch("--build-cpu")]
    public string? BuildCpu { get; set; }

    [CommandSwitch("--build-env")]
    public string? BuildEnv { get; set; }

    [CommandSwitch("--build-memory")]
    public string? BuildMemory { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

    [CommandSwitch("--certificates")]
    public string? Certificates { get; set; }

    [BooleanCommandSwitch("--disable-validation")]
    public bool? DisableValidation { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--source-path")]
    public string? SourcePath { get; set; }
}