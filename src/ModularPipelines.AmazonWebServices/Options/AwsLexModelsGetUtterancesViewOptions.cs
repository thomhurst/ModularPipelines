using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-models", "get-utterances-view")]
public record AwsLexModelsGetUtterancesViewOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--bot-versions")] string[] BotVersions,
[property: CliOption("--status-type")] string StatusType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}