using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "describe-model-version")]
public record AwsLookoutequipmentDescribeModelVersionOptions(
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--model-version")] long ModelVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}