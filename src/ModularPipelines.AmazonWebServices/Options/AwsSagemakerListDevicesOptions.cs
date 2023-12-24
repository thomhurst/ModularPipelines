using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-devices")]
public record AwsSagemakerListDevicesOptions : AwsOptions
{
    [CommandSwitch("--latest-heartbeat-after")]
    public long? LatestHeartbeatAfter { get; set; }

    [CommandSwitch("--model-name")]
    public string? ModelName { get; set; }

    [CommandSwitch("--device-fleet-name")]
    public string? DeviceFleetName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}