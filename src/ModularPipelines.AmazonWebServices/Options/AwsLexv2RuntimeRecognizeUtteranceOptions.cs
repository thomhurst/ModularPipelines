using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-runtime", "recognize-utterance")]
public record AwsLexv2RuntimeRecognizeUtteranceOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-alias-id")] string BotAliasId,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--request-content-type")] string RequestContentType
) : AwsOptions
{
    [CliOption("--session-state")]
    public string? SessionState { get; set; }

    [CliOption("--request-attributes")]
    public string? RequestAttributes { get; set; }

    [CliOption("--response-content-type")]
    public string? ResponseContentType { get; set; }

    [CliOption("--input-stream")]
    public string? InputStream { get; set; }
}