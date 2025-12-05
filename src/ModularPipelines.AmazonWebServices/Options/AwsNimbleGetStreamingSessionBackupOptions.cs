using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "get-streaming-session-backup")]
public record AwsNimbleGetStreamingSessionBackupOptions(
[property: CliOption("--backup-id")] string BackupId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}