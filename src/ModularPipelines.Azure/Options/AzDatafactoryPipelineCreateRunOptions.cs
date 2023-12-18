using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "pipeline", "create-run")]
public record AzDatafactoryPipelineCreateRunOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--is-recovery")]
    public bool? IsRecovery { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--reference-pipeline-run-id")]
    public string? ReferencePipelineRunId { get; set; }

    [CommandSwitch("--start-activity-name")]
    public string? StartActivityName { get; set; }

    [BooleanCommandSwitch("--start-from-failure")]
    public bool? StartFromFailure { get; set; }
}