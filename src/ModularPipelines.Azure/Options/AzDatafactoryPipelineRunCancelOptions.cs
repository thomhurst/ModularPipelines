using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "pipeline-run", "cancel")]
public record AzDatafactoryPipelineRunCancelOptions : AzOptions
{
    [CommandSwitch("--factory-name")]
    public string? FactoryName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--is-recursive")]
    public bool? IsRecursive { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}