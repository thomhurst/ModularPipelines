using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "association", "update")]
public record AzMonitorDataCollectionRuleAssociationUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--endpoint-id")]
    public string? EndpointId { get; set; }

    [CommandSwitch("--rule-id")]
    public string? RuleId { get; set; }
}