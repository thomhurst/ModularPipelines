using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "describe-label")]
public record AwsLookoutequipmentDescribeLabelOptions(
[property: CommandSwitch("--label-group-name")] string LabelGroupName,
[property: CommandSwitch("--label-id")] string LabelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}