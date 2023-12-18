using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "association", "show")]
public record AzMonitorDataCollectionRuleAssociationShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions;