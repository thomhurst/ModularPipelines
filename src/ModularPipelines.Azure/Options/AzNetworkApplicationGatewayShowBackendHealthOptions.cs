using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "show-backend-health")]
public record AzNetworkApplicationGatewayShowBackendHealthOptions : AzOptions
{
    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [BooleanCommandSwitch("--host-name-from-http-settings")]
    public bool? HostNameFromHttpSettings { get; set; }

    [CommandSwitch("--http-settings")]
    public string? HttpSettings { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--match-body")]
    public string? MatchBody { get; set; }

    [CommandSwitch("--match-status-codes")]
    public string? MatchStatusCodes { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}