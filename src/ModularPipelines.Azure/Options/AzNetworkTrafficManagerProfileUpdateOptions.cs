using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "traffic-manager", "profile", "update")]
public record AzNetworkTrafficManagerProfileUpdateOptions : AzOptions
{
    [CliOption("--custom-headers")]
    public string? CustomHeaders { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--max-failures")]
    public string? MaxFailures { get; set; }

    [CliOption("--max-return")]
    public string? MaxReturn { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--routing-method")]
    public string? RoutingMethod { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--status-code-ranges")]
    public string? StatusCodeRanges { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}