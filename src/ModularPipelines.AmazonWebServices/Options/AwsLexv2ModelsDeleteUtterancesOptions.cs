using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "delete-utterances")]
public record AwsLexv2ModelsDeleteUtterancesOptions(
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--locale-id")]
    public string? LocaleId { get; set; }

    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}