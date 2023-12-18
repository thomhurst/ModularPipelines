using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "origin", "create")]
public record AzAfdOriginCreateOptions(
[property: BooleanCommandSwitch("--enabled-state")] bool EnabledState,
[property: CommandSwitch("--host-name")] string HostName,
[property: CommandSwitch("--origin-group-name")] string OriginGroupName,
[property: CommandSwitch("--origin-name")] string OriginName,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [BooleanCommandSwitch("--enforce-certificate-name-check")]
    public bool? EnforceCertificateNameCheck { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--https-port")]
    public string? HttpsPort { get; set; }

    [CommandSwitch("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

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

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}