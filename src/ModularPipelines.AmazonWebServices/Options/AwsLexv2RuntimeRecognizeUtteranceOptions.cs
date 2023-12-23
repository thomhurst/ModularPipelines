using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-runtime", "recognize-utterance")]
public record AwsLexv2RuntimeRecognizeUtteranceOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-alias-id")] string BotAliasId,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--request-content-type")] string RequestContentType
) : AwsOptions
{
    [CommandSwitch("--session-state")]
    public string? SessionState { get; set; }

    [CommandSwitch("--request-attributes")]
    public string? RequestAttributes { get; set; }

    [CommandSwitch("--response-content-type")]
    public string? ResponseContentType { get; set; }

    [CommandSwitch("--input-stream")]
    public string? InputStream { get; set; }
}