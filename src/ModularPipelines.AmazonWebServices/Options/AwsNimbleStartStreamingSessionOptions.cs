using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "start-streaming-session")]
public record AwsNimbleStartStreamingSessionOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--backup-id")]
    public string? BackupId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}