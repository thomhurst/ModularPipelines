using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-slot")]
public record AwsLexv2ModelsCreateSlotOptions(
[property: CliOption("--slot-name")] string SlotName,
[property: CliOption("--value-elicitation-setting")] string ValueElicitationSetting,
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--intent-id")] string IntentId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--slot-type-id")]
    public string? SlotTypeId { get; set; }

    [CliOption("--obfuscation-setting")]
    public string? ObfuscationSetting { get; set; }

    [CliOption("--multiple-values-setting")]
    public string? MultipleValuesSetting { get; set; }

    [CliOption("--sub-slot-setting")]
    public string? SubSlotSetting { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}