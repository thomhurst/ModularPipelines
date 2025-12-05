using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-devices")]
public record AwsSagemakerListDevicesOptions : AwsOptions
{
    [CliOption("--latest-heartbeat-after")]
    public long? LatestHeartbeatAfter { get; set; }

    [CliOption("--model-name")]
    public string? ModelName { get; set; }

    [CliOption("--device-fleet-name")]
    public string? DeviceFleetName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}