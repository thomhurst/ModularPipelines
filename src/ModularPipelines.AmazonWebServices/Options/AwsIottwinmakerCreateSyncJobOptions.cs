using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "create-sync-job")]
public record AwsIottwinmakerCreateSyncJobOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--sync-source")] string SyncSource,
[property: CliOption("--sync-role")] string SyncRole
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}