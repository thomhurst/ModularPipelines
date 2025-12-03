using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "traffic-manager", "profile", "create")]
public record AzNetworkTrafficManagerProfileCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--routing-method")] string RoutingMethod,
[property: CliOption("--unique-dns-name")] string UniqueDnsName
) : AzOptions
{
    [CliOption("--custom-headers")]
    public string? CustomHeaders { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--max-failures")]
    public string? MaxFailures { get; set; }

    [CliOption("--max-return")]
    public string? MaxReturn { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--status-code-ranges")]
    public string? StatusCodeRanges { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}