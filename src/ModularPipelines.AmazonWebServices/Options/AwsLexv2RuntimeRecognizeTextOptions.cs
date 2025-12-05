using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-runtime", "recognize-text")]
public record AwsLexv2RuntimeRecognizeTextOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-alias-id")] string BotAliasId,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--text")] string Text
) : AwsOptions
{
    [CliOption("--session-state")]
    public string? SessionState { get; set; }

    [CliOption("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}