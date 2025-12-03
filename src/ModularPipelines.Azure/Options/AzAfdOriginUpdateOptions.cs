using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "origin", "update")]
public record AzAfdOriginUpdateOptions : AzOptions
{
    [CliFlag("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CliFlag("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CliFlag("--enforce-certificate-name-check")]
    public bool? EnforceCertificateNameCheck { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--https-port")]
    public string? HttpsPort { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--origin-group-name")]
    public string? OriginGroupName { get; set; }

    [CliOption("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CliOption("--origin-name")]
    public string? OriginName { get; set; }

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

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}