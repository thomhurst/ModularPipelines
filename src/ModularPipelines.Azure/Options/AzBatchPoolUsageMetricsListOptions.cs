using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "pool", "usage-metrics", "list")]
public record AzBatchPoolUsageMetricsListOptions : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}