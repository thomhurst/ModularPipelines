using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "delete-bot-version")]
public record AwsLexModelsDeleteBotVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--bot-version")] string BotVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}