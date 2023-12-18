using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "performance-counter", "add")]
public record AzMonitorDataCollectionRulePerformanceCounterAddOptions(
[property: CommandSwitch("--counter-specifiers")] int CounterSpecifiers,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--sampling-frequency")] string SamplingFrequency,
[property: CommandSwitch("--streams")] string Streams
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}