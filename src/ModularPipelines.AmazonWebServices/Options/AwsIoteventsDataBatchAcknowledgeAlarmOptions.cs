using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "batch-acknowledge-alarm")]
public record AwsIoteventsDataBatchAcknowledgeAlarmOptions(
[property: CliOption("--acknowledge-action-requests")] string[] AcknowledgeActionRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}