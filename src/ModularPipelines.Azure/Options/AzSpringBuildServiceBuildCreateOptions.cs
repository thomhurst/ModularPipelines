using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "build-service", "build", "create")]
public record AzSpringBuildServiceBuildCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--apms")]
    public string? Apms { get; set; }

    [CliOption("--artifact-path")]
    public string? ArtifactPath { get; set; }

    [CliOption("--build-cpu")]
    public string? BuildCpu { get; set; }

    [CliOption("--build-env")]
    public string? BuildEnv { get; set; }

    [CliOption("--build-memory")]
    public string? BuildMemory { get; set; }

    [CliOption("--builder")]
    public string? Builder { get; set; }

    [CliOption("--certificates")]
    public string? Certificates { get; set; }

    [CliFlag("--disable-validation")]
    public bool? DisableValidation { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--source-path")]
    public string? SourcePath { get; set; }
}