using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "create")]
public record GcloudComputeNetworksSubnetsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network,
[property: CliOption("--range")] string Range
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-flow-logs")]
    public bool? EnableFlowLogs { get; set; }

    [CliFlag("--enable-private-ip-google-access")]
    public bool? EnablePrivateIpGoogleAccess { get; set; }

    [CliOption("--ipv6-access-type")]
    public string? Ipv6AccessType { get; set; }

    [CliOption("--logging-aggregation-interval")]
    public string? LoggingAggregationInterval { get; set; }

    [CliOption("--logging-filter-expr")]
    public string? LoggingFilterExpr { get; set; }

    [CliOption("--logging-flow-sampling")]
    public string? LoggingFlowSampling { get; set; }

    [CliOption("--logging-metadata")]
    public string? LoggingMetadata { get; set; }

    [CliOption("--logging-metadata-fields")]
    public string[]? LoggingMetadataFields { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--purpose")]
    public string? Purpose { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--secondary-range")]
    public string[]? SecondaryRange { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }
}