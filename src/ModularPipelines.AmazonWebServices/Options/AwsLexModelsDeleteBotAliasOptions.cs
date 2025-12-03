using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "delete-bot-alias")]
public record AwsLexModelsDeleteBotAliasOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--bot-name")] string BotName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}