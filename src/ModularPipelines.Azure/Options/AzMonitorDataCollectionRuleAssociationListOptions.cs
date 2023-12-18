using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "association", "list")]
public record AzMonitorDataCollectionRuleAssociationListOptions : AzOptions
{
    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }
}