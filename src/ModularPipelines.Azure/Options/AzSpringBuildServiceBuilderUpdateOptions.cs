using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "build-service", "builder", "update")]
public record AzSpringBuildServiceBuilderUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--builder-file")]
    public string? BuilderFile { get; set; }

    [CliOption("--builder-json")]
    public string? BuilderJson { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}