using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "query-assistant")]
public record AwsQconnectQueryAssistantOptions(
[property: CliOption("--assistant-id")] string AssistantId,
[property: CliOption("--query-text")] string QueryText
) : AwsOptions
{
    [CliOption("--query-condition")]
    public string[]? QueryCondition { get; set; }

    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}