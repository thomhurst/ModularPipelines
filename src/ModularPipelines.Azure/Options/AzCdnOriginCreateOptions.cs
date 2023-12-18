using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "origin", "create")]
public record AzCdnOriginCreateOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--host-name")] string HostName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--https-port")]
    public string? HttpsPort { get; set; }

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

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}