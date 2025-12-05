using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "data-collection", "rule", "association", "show")]
public record AzMonitorDataCollectionRuleAssociationShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource")] string Resource
) : AzOptions;