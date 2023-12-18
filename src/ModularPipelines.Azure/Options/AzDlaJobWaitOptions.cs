using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "job", "wait")]
public record AzDlaJobWaitOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-wait-time-sec")]
    public string? MaxWaitTimeSec { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--wait-interval-sec")]
    public string? WaitIntervalSec { get; set; }
}