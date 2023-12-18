using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "backend-pool", "backend", "add")]
public record AzNetworkFrontDoorBackendPoolBackendAddOptions(
[property: CommandSwitch("--address")] string Address,
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--approval-message")]
    public string? ApprovalMessage { get; set; }

    [CommandSwitch("--backend-host-header")]
    public string? BackendHostHeader { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--https-port")]
    public string? HttpsPort { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--private-link-alias")]
    public string? PrivateLinkAlias { get; set; }

    [CommandSwitch("--private-link-location")]
    public string? PrivateLinkLocation { get; set; }

    [CommandSwitch("--private-link-resource-id")]
    public string? PrivateLinkResourceId { get; set; }

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}