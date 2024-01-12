using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "create")]
public record GcloudComputeNetworksSubnetsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--range")] string Range
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-flow-logs")]
    public bool? EnableFlowLogs { get; set; }

    [BooleanCommandSwitch("--enable-private-ip-google-access")]
    public bool? EnablePrivateIpGoogleAccess { get; set; }

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

    [CommandSwitch("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CommandSwitch("--purpose")]
    public string? Purpose { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--secondary-range")]
    public string[]? SecondaryRange { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }
}