using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "origin", "update")]
public record AzAfdOriginUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [BooleanCommandSwitch("--enabled-state")]
    public bool? EnabledState { get; set; }

    [BooleanCommandSwitch("--enforce-certificate-name-check")]
    public bool? EnforceCertificateNameCheck { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--https-port")]
    public string? HttpsPort { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--origin-group-name")]
    public string? OriginGroupName { get; set; }

    [CommandSwitch("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CommandSwitch("--origin-name")]
    public string? OriginName { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--private-link-location")]
    public string? PrivateLinkLocation { get; set; }

    [CommandSwitch("--private-link-request-message")]
    public string? PrivateLinkRequestMessage { get; set; }

    [CommandSwitch("--private-link-resource")]
    public string? PrivateLinkResource { get; set; }

    [CommandSwitch("--private-link-sub-resource-type")]
    public string? PrivateLinkSubResourceType { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}

