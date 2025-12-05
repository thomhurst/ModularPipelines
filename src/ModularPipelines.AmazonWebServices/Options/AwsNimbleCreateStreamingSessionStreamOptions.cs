using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "create-streaming-session-stream")]
public record AwsNimbleCreateStreamingSessionStreamOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--expiration-in-seconds")]
    public int? ExpirationInSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}