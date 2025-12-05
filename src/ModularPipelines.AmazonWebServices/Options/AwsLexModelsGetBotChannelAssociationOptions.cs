using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "get-bot-channel-association")]
public record AwsLexModelsGetBotChannelAssociationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--bot-alias")] string BotAlias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}