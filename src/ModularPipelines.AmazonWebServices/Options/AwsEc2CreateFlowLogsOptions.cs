using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-flow-logs")]
public record AwsEc2CreateFlowLogsOptions(
[property: CommandSwitch("--resource-ids")] string[] ResourceIds,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--deliver-logs-permission-arn")]
    public string? DeliverLogsPermissionArn { get; set; }

    [CommandSwitch("--deliver-cross-account-role")]
    public string? DeliverCrossAccountRole { get; set; }

    [CommandSwitch("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CommandSwitch("--traffic-type")]
    public string? TrafficType { get; set; }

    [CommandSwitch("--log-destination-type")]
    public string? LogDestinationType { get; set; }

    [CommandSwitch("--log-destination")]
    public string? LogDestination { get; set; }

    [CommandSwitch("--log-format")]
    public string? LogFormat { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--max-aggregation-interval")]
    public int? MaxAggregationInterval { get; set; }

    [CommandSwitch("--destination-options")]
    public string? DestinationOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}