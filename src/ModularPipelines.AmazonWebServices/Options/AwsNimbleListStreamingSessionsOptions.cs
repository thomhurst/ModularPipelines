using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "list-streaming-sessions")]
public record AwsNimbleListStreamingSessionsOptions(
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--created-by")]
    public string? CreatedBy { get; set; }

    [CliOption("--owned-by")]
    public string? OwnedBy { get; set; }

    [CliOption("--session-ids")]
    public string? SessionIds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}