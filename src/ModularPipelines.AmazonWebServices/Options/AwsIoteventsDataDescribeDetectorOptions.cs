using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "describe-detector")]
public record AwsIoteventsDataDescribeDetectorOptions(
[property: CommandSwitch("--detector-model-name")] string DetectorModelName
) : AwsOptions
{
    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}