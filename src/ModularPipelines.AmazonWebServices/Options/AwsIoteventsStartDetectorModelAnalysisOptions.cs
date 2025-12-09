using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "start-detector-model-analysis")]
public record AwsIoteventsStartDetectorModelAnalysisOptions(
[property: CliOption("--detector-model-definition")] string DetectorModelDefinition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}