using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "origin", "create")]
public record AzAfdOriginCreateOptions(
[property: CliFlag("--enabled-state")] bool EnabledState,
[property: CliOption("--host-name")] string HostName,
[property: CliOption("--origin-group-name")] string OriginGroupName,
[property: CliOption("--origin-name")] string OriginName,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CliFlag("--enforce-certificate-name-check")]
    public bool? EnforceCertificateNameCheck { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--https-port")]
    public string? HttpsPort { get; set; }

    [CliOption("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--private-link-location")]
    public string? PrivateLinkLocation { get; set; }

    [CliOption("--private-link-request-message")]
    public string? PrivateLinkRequestMessage { get; set; }

    [CliOption("--private-link-resource")]
    public string? PrivateLinkResource { get; set; }

    [CliOption("--private-link-sub-resource-type")]
    public string? PrivateLinkSubResourceType { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}