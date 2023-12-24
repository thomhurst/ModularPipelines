using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "delete-bot-channel-association")]
public record AwsLexModelsDeleteBotChannelAssociationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--bot-alias")] string BotAlias
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}