using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "delete-utterances")]
public record AwsLexModelsDeleteUtterancesOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}