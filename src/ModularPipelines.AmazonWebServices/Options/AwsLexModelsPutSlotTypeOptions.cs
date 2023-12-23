using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "put-slot-type")]
public record AwsLexModelsPutSlotTypeOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enumeration-values")]
    public string[]? EnumerationValues { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--value-selection-strategy")]
    public string? ValueSelectionStrategy { get; set; }

    [CommandSwitch("--parent-slot-type-signature")]
    public string? ParentSlotTypeSignature { get; set; }

    [CommandSwitch("--slot-type-configurations")]
    public string[]? SlotTypeConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}