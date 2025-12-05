using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lex-runtime", "put-session")]
public record AwsLexRuntimePutSessionOptions(
[property: CliOption("--bot-name")] string BotName,
[property: CliOption("--bot-alias")] string BotAlias,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--session-attributes")]
    public IEnumerable<KeyValue>? SessionAttributes { get; set; }

    [CliOption("--dialog-action")]
    public string? DialogAction { get; set; }

    [CliOption("--recent-intent-summary-view")]
    public string[]? RecentIntentSummaryView { get; set; }

    [CliOption("--accept")]
    public string? Accept { get; set; }

    [CliOption("--active-contexts")]
    public string[]? ActiveContexts { get; set; }
}