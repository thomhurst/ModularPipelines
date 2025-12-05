using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "put-slot-type")]
public record AwsLexModelsPutSlotTypeOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enumeration-values")]
    public string[]? EnumerationValues { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--value-selection-strategy")]
    public string? ValueSelectionStrategy { get; set; }

    [CliOption("--parent-slot-type-signature")]
    public string? ParentSlotTypeSignature { get; set; }

    [CliOption("--slot-type-configurations")]
    public string[]? SlotTypeConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}