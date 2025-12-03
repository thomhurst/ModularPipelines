using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "show-backend-health")]
public record AzNetworkApplicationGatewayShowBackendHealthOptions : AzOptions
{
    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliFlag("--host-name-from-http-settings")]
    public bool? HostNameFromHttpSettings { get; set; }

    [CliOption("--http-settings")]
    public string? HttpSettings { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--match-body")]
    public string? MatchBody { get; set; }

    [CliOption("--match-status-codes")]
    public string? MatchStatusCodes { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}