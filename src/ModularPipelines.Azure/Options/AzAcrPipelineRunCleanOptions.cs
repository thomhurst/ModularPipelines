using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "pipeline-run", "clean")]
public record AzAcrPipelineRunCleanOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }
}