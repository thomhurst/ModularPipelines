using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "rule", "data-flow", "add")]
public record AzMonitorDataCollectionRuleDataFlowAddOptions(
[property: CliOption("--destinations")] string Destinations,
[property: CliOption("--streams")] string Streams
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}