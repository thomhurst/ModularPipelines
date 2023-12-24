using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "delete-intent")]
public record AwsLexv2ModelsDeleteIntentOptions(
[property: CommandSwitch("--intent-id")] string IntentId,
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}