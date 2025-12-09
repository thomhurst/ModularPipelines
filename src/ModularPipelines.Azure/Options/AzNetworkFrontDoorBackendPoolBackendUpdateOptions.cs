using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "backend-pool", "backend", "update")]
public record AzNetworkFrontDoorBackendPoolBackendUpdateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--index")] string Index,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--approval-message")]
    public string? ApprovalMessage { get; set; }

    [CliOption("--backend-host-header")]
    public string? BackendHostHeader { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--https-port")]
    public string? HttpsPort { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--private-link-alias")]
    public string? PrivateLinkAlias { get; set; }

    [CliOption("--private-link-location")]
    public string? PrivateLinkLocation { get; set; }

    [CliOption("--private-link-resource-id")]
    public string? PrivateLinkResourceId { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}