using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "get-resource-sync-status")]
public record AwsCodestarConnectionsGetResourceSyncStatusOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}