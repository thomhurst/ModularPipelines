using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-slot-type")]
public record AwsLexv2ModelsCreateSlotTypeOptions(
[property: CommandSwitch("--slot-type-name")] string SlotTypeName,
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--slot-type-values")]
    public string[]? SlotTypeValues { get; set; }

    [CommandSwitch("--value-selection-setting")]
    public string? ValueSelectionSetting { get; set; }

    [CommandSwitch("--parent-slot-type-signature")]
    public string? ParentSlotTypeSignature { get; set; }

    [CommandSwitch("--external-source-setting")]
    public string? ExternalSourceSetting { get; set; }

    [CommandSwitch("--composite-slot-type-setting")]
    public string? CompositeSlotTypeSetting { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}