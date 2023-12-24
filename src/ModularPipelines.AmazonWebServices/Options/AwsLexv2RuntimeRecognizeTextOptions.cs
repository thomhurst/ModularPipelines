using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-runtime", "recognize-text")]
public record AwsLexv2RuntimeRecognizeTextOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-alias-id")] string BotAliasId,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--text")] string Text
) : AwsOptions
{
    [CommandSwitch("--session-state")]
    public string? SessionState { get; set; }

    [CommandSwitch("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}