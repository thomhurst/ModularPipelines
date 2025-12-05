using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-slot-type")]
public record AwsLexv2ModelsUpdateSlotTypeOptions(
[property: CliOption("--slot-type-id")] string SlotTypeId,
[property: CliOption("--slot-type-name")] string SlotTypeName,
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--slot-type-values")]
    public string[]? SlotTypeValues { get; set; }

    [CliOption("--value-selection-setting")]
    public string? ValueSelectionSetting { get; set; }

    [CliOption("--parent-slot-type-signature")]
    public string? ParentSlotTypeSignature { get; set; }

    [CliOption("--external-source-setting")]
    public string? ExternalSourceSetting { get; set; }

    [CliOption("--composite-slot-type-setting")]
    public string? CompositeSlotTypeSetting { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}