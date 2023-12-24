using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "describe-slot")]
public record AwsLexv2ModelsDescribeSlotOptions(
[property: CommandSwitch("--slot-id")] string SlotId,
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--intent-id")] string IntentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}