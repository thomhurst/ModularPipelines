using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "describe-detector-model")]
public record AwsIoteventsDescribeDetectorModelOptions(
[property: CliOption("--detector-model-name")] string DetectorModelName
) : AwsOptions
{
    [CliOption("--detector-model-version")]
    public string? DetectorModelVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}