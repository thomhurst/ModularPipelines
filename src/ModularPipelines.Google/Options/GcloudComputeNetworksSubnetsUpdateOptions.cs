using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "update")]
public record GcloudComputeNetworksSubnetsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [CommandSwitch("--ipv6-access-type")]
    public string? Ipv6AccessType { get; set; }

    [CommandSwitch("--logging-aggregation-interval")]
    public string? LoggingAggregationInterval { get; set; }

    [CommandSwitch("--logging-filter-expr")]
    public string? LoggingFilterExpr { get; set; }

    [CommandSwitch("--logging-flow-sampling")]
    public string? LoggingFlowSampling { get; set; }

    [CommandSwitch("--logging-metadata")]
    public string? LoggingMetadata { get; set; }

    [CommandSwitch("--logging-metadata-fields")]
    public string[]? LoggingMetadataFields { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }

    [CommandSwitch("--add-secondary-ranges")]
    public string[]? AddSecondaryRanges { get; set; }

    [CommandSwitch("--[no-]enable-flow-logs")]
    public string[]? NoEnableFlowLogs { get; set; }

    [CommandSwitch("--[no-]enable-private-ip-google-access")]
    public string[]? NoEnablePrivateIpGoogleAccess { get; set; }

    [CommandSwitch("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CommandSwitch("--purpose")]
    public string? Purpose { get; set; }

    [BooleanCommandSwitch("REGIONAL_MANAGED_PROXY")]
    public bool? RegionalManagedProxy { get; set; }

    [CommandSwitch("--remove-secondary-ranges")]
    public string[]? RemoveSecondaryRanges { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [BooleanCommandSwitch("ACTIVE")]
    public bool? Active { get; set; }
}