using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "data-collection", "rule", "association", "delete")]
public record AzMonitorDataCollectionRuleAssociationDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}