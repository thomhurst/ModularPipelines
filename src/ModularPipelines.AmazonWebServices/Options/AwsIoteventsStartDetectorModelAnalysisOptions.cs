using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "start-detector-model-analysis")]
public record AwsIoteventsStartDetectorModelAnalysisOptions(
[property: CommandSwitch("--detector-model-definition")] string DetectorModelDefinition
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}