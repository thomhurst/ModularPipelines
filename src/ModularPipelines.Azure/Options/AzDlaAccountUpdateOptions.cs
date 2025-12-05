using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "account", "update")]
public record AzDlaAccountUpdateOptions : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliFlag("--allow-azure-ips")]
    public bool? AllowAzureIps { get; set; }

    [CliOption("--firewall-state")]
    public string? FirewallState { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-degree-of-parallelism")]
    public string? MaxDegreeOfParallelism { get; set; }

    [CliOption("--max-job-count")]
    public int? MaxJobCount { get; set; }

    [CliOption("--query-store-retention")]
    public string? QueryStoreRetention { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }
}