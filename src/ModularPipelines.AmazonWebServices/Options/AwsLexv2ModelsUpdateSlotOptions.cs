using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "update-slot")]
public record AwsLexv2ModelsUpdateSlotOptions(
[property: CommandSwitch("--slot-id")] string SlotId,
[property: CommandSwitch("--slot-name")] string SlotName,
[property: CommandSwitch("--value-elicitation-setting")] string ValueElicitationSetting,
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--intent-id")] string IntentId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--slot-type-id")]
    public string? SlotTypeId { get; set; }

    [CommandSwitch("--obfuscation-setting")]
    public string? ObfuscationSetting { get; set; }

    [CommandSwitch("--multiple-values-setting")]
    public string? MultipleValuesSetting { get; set; }

    [CommandSwitch("--sub-slot-setting")]
    public string? SubSlotSetting { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}