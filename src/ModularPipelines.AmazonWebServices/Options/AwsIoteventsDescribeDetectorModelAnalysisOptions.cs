using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "describe-detector-model-analysis")]
public record AwsIoteventsDescribeDetectorModelAnalysisOptions(
[property: CliOption("--analysis-id")] string AnalysisId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}