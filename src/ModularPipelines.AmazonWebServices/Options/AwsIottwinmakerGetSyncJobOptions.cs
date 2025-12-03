using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "get-sync-job")]
public record AwsIottwinmakerGetSyncJobOptions(
[property: CliOption("--sync-source")] string SyncSource
) : AwsOptions
{
    [CliOption("--workspace-id")]
    public string? WorkspaceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}