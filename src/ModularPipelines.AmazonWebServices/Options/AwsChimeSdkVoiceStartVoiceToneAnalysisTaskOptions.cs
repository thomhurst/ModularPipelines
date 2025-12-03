using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "start-voice-tone-analysis-task")]
public record AwsChimeSdkVoiceStartVoiceToneAnalysisTaskOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--transaction-id")] string TransactionId,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}