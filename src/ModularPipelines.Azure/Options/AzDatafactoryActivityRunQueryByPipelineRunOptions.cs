using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "activity-run", "query-by-pipeline-run")]
public record AzDatafactoryActivityRunQueryByPipelineRunOptions(
[property: CommandSwitch("--last-updated-after")] string LastUpdatedAfter,
[property: CommandSwitch("--last-updated-before")] string LastUpdatedBefore
) : AzOptions
{
    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CommandSwitch("--factory-name")]
    public string? FactoryName { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

