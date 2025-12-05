using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-runtime", "put-session")]
public record AwsLexv2RuntimePutSessionOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-alias-id")] string BotAliasId,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--session-state")] string SessionState
) : AwsOptions
{
    [CliOption("--messages")]
    public string[]? Messages { get; set; }

    [CliOption("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CliOption("--response-content-type")]
    public string? ResponseContentType { get; set; }
}