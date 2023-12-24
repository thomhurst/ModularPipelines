using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lex-runtime", "put-session")]
public record AwsLexRuntimePutSessionOptions(
[property: CommandSwitch("--bot-name")] string BotName,
[property: CommandSwitch("--bot-alias")] string BotAlias,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--session-attributes")]
    public IEnumerable<KeyValue>? SessionAttributes { get; set; }

    [CommandSwitch("--dialog-action")]
    public string? DialogAction { get; set; }

    [CommandSwitch("--recent-intent-summary-view")]
    public string[]? RecentIntentSummaryView { get; set; }

    [CommandSwitch("--accept")]
    public string? Accept { get; set; }

    [CommandSwitch("--active-contexts")]
    public string[]? ActiveContexts { get; set; }
}