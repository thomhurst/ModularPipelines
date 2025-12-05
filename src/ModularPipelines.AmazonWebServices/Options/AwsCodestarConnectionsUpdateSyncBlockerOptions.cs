using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "update-sync-blocker")]
public record AwsCodestarConnectionsUpdateSyncBlockerOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--sync-type")] string SyncType,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resolved-reason")] string ResolvedReason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}