using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cdn", "origin", "create")]
public record AzCdnOriginCreateOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--host-name")] string HostName,
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--https-port")]
    public string? HttpsPort { get; set; }

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

    [CliOption("--weight")]
    public string? Weight { get; set; }
}