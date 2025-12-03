using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-flow-logs")]
public record AwsEc2CreateFlowLogsOptions(
[property: CliOption("--resource-ids")] string[] ResourceIds,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--deliver-logs-permission-arn")]
    public string? DeliverLogsPermissionArn { get; set; }

    [CliOption("--deliver-cross-account-role")]
    public string? DeliverCrossAccountRole { get; set; }

    [CliOption("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CliOption("--traffic-type")]
    public string? TrafficType { get; set; }

    [CliOption("--log-destination-type")]
    public string? LogDestinationType { get; set; }

    [CliOption("--log-destination")]
    public string? LogDestination { get; set; }

    [CliOption("--log-format")]
    public string? LogFormat { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--max-aggregation-interval")]
    public int? MaxAggregationInterval { get; set; }

    [CliOption("--destination-options")]
    public string? DestinationOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}