using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "update-label-group")]
public record AwsLookoutequipmentUpdateLabelGroupOptions(
[property: CommandSwitch("--label-group-name")] string LabelGroupName
) : AwsOptions
{
    [CommandSwitch("--fault-codes")]
    public string[]? FaultCodes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}