using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "batch-enable-alarm")]
public record AwsIoteventsDataBatchEnableAlarmOptions(
[property: CliOption("--enable-action-requests")] string[] EnableActionRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}