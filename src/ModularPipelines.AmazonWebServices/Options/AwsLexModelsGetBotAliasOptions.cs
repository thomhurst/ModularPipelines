using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-models", "get-bot-alias")]
public record AwsLexModelsGetBotAliasOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--bot-name")] string BotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}