using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "origin", "update")]
public record AzCdnOriginUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--https-port")]
    public string? HttpsPort { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--private-link-approval-message")]
    public string? PrivateLinkApprovalMessage { get; set; }

    [CommandSwitch("--private-link-location")]
    public string? PrivateLinkLocation { get; set; }

    [CommandSwitch("--private-link-resource-id")]
    public string? PrivateLinkResourceId { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}

