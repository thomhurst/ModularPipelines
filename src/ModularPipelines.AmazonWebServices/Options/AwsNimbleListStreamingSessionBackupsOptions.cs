using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "list-streaming-session-backups")]
public record AwsNimbleListStreamingSessionBackupsOptions(
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--owned-by")]
    public string? OwnedBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}