using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "job", "wait")]
public record AzDlaJobWaitOptions(
[property: CliOption("--job-id")] string JobId
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-wait-time-sec")]
    public string? MaxWaitTimeSec { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--wait-interval-sec")]
    public string? WaitIntervalSec { get; set; }
}