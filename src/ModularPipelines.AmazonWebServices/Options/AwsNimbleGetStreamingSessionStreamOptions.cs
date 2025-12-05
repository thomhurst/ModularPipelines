using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "get-streaming-session-stream")]
public record AwsNimbleGetStreamingSessionStreamOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--stream-id")] string StreamId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}