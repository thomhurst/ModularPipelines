using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "update")]
public record GcloudComputeNetworksSubnetsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--drain-timeout")]
    public string? DrainTimeout { get; set; }

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

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }

    [CliOption("--add-secondary-ranges")]
    public string[]? AddSecondaryRanges { get; set; }

    [CliOption("--[no-]enable-flow-logs")]
    public string[]? NoEnableFlowLogs { get; set; }

    [CliOption("--[no-]enable-private-ip-google-access")]
    public string[]? NoEnablePrivateIpGoogleAccess { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--purpose")]
    public string? Purpose { get; set; }

    [CliFlag("REGIONAL_MANAGED_PROXY")]
    public bool? RegionalManagedProxy { get; set; }

    [CliOption("--remove-secondary-ranges")]
    public string[]? RemoveSecondaryRanges { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliFlag("ACTIVE")]
    public bool? Active { get; set; }
}