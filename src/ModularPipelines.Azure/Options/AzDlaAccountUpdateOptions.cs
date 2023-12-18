using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "account", "update")]
public record AzDlaAccountUpdateOptions : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [BooleanCommandSwitch("--allow-azure-ips")]
    public bool? AllowAzureIps { get; set; }

    [CommandSwitch("--firewall-state")]
    public string? FirewallState { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-degree-of-parallelism")]
    public string? MaxDegreeOfParallelism { get; set; }

    [CommandSwitch("--max-job-count")]
    public int? MaxJobCount { get; set; }

    [CommandSwitch("--query-store-retention")]
    public string? QueryStoreRetention { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }
}