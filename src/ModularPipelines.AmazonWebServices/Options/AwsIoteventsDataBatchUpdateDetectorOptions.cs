using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "batch-update-detector")]
public record AwsIoteventsDataBatchUpdateDetectorOptions(
[property: CliOption("--detectors")] string[] Detectors
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}