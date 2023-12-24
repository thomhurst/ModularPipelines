using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "wait", "bot-alias-available")]
public record AwsLexv2ModelsWaitBotAliasAvailableOptions(
[property: CommandSwitch("--bot-alias-id")] string BotAliasId,
[property: CommandSwitch("--bot-id")] string BotId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}