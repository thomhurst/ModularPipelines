using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "data-collection", "rule", "association", "update")]
public record AzMonitorDataCollectionRuleAssociationUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--endpoint-id")]
    public string? EndpointId { get; set; }

    [CliOption("--rule-id")]
    public string? RuleId { get; set; }
}