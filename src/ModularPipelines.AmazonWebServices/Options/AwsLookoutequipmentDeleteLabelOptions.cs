using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "delete-label")]
public record AwsLookoutequipmentDeleteLabelOptions(
[property: CommandSwitch("--label-group-name")] string LabelGroupName,
[property: CommandSwitch("--label-id")] string LabelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}