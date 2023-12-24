using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-runtime", "put-session")]
public record AwsLexv2RuntimePutSessionOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-alias-id")] string BotAliasId,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--session-state")] string SessionState
) : AwsOptions
{
    [CommandSwitch("--messages")]
    public string[]? Messages { get; set; }

    [CommandSwitch("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CommandSwitch("--response-content-type")]
    public string? ResponseContentType { get; set; }
}