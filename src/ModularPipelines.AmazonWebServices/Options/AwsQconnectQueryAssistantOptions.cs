using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "query-assistant")]
public record AwsQconnectQueryAssistantOptions(
[property: CommandSwitch("--assistant-id")] string AssistantId,
[property: CommandSwitch("--query-text")] string QueryText
) : AwsOptions
{
    [CommandSwitch("--query-condition")]
    public string[]? QueryCondition { get; set; }

    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}