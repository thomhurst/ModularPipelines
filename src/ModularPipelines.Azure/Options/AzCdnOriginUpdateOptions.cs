using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cdn", "origin", "update")]
public record AzCdnOriginUpdateOptions : AzOptions
{
    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--https-port")]
    public string? HttpsPort { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--origin-host-header")]
    public string? OriginHostHeader { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--private-link-approval-message")]
    public string? PrivateLinkApprovalMessage { get; set; }

    [CliOption("--private-link-location")]
    public string? PrivateLinkLocation { get; set; }

    [CliOption("--private-link-resource-id")]
    public string? PrivateLinkResourceId { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}