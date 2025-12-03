using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "job", "start")]
public record AzStreamAnalyticsJobStartOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--output-start-mode")]
    public string? OutputStartMode { get; set; }

    [CliOption("--output-start-time")]
    public string? OutputStartTime { get; set; }
}