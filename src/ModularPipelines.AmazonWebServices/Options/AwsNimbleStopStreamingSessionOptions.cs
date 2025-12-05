using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "stop-streaming-session")]
public record AwsNimbleStopStreamingSessionOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--volume-retention-mode")]
    public string? VolumeRetentionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}