using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "create-label-group")]
public record AwsLookoutequipmentCreateLabelGroupOptions(
[property: CommandSwitch("--label-group-name")] string LabelGroupName
) : AwsOptions
{
    [CommandSwitch("--fault-codes")]
    public string[]? FaultCodes { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}