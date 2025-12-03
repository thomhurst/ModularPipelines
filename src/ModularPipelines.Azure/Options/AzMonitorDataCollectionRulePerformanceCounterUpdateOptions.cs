using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "rule", "performance-counter", "update")]
public record AzMonitorDataCollectionRulePerformanceCounterUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--counter-specifiers")]
    public int? CounterSpecifiers { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--sampling-frequency")]
    public string? SamplingFrequency { get; set; }

    [CliOption("--streams")]
    public string? Streams { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}