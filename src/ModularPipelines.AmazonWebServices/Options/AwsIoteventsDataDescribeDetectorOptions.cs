using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "describe-detector")]
public record AwsIoteventsDataDescribeDetectorOptions(
[property: CliOption("--detector-model-name")] string DetectorModelName
) : AwsOptions
{
    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}