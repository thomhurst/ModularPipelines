using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "get-sync-job")]
public record AwsIottwinmakerGetSyncJobOptions(
[property: CommandSwitch("--sync-source")] string SyncSource
) : AwsOptions
{
    [CommandSwitch("--workspace-id")]
    public string? WorkspaceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}