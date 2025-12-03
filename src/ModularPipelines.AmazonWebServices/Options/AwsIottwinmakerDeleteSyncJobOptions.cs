using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "delete-sync-job")]
public record AwsIottwinmakerDeleteSyncJobOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--sync-source")] string SyncSource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}