using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "association", "delete")]
public record AzMonitorDataCollectionRuleAssociationDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}