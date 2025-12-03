using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "rule", "performance-counter", "add")]
public record AzMonitorDataCollectionRulePerformanceCounterAddOptions(
[property: CliOption("--counter-specifiers")] int CounterSpecifiers,
[property: CliOption("--name")] string Name,
[property: CliOption("--sampling-frequency")] string SamplingFrequency,
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