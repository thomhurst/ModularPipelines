using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "pipeline-run", "query-by-factory")]
public record AzDatafactoryPipelineRunQueryByFactoryOptions(
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

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}