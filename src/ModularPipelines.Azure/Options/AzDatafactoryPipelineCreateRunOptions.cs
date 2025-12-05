using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "pipeline", "create-run")]
public record AzDatafactoryPipelineCreateRunOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--is-recovery")]
    public bool? IsRecovery { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--reference-pipeline-run-id")]
    public string? ReferencePipelineRunId { get; set; }

    [CliOption("--start-activity-name")]
    public string? StartActivityName { get; set; }

    [CliFlag("--start-from-failure")]
    public bool? StartFromFailure { get; set; }
}