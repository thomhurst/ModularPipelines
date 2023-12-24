using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "describe-detector-model")]
public record AwsIoteventsDescribeDetectorModelOptions(
[property: CommandSwitch("--detector-model-name")] string DetectorModelName
) : AwsOptions
{
    [CommandSwitch("--detector-model-version")]
    public string? DetectorModelVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}