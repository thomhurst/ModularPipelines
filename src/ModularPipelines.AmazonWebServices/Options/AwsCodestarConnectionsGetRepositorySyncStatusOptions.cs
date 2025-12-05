using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "get-repository-sync-status")]
public record AwsCodestarConnectionsGetRepositorySyncStatusOptions(
[property: CliOption("--branch")] string Branch,
[property: CliOption("--repository-link-id")] string RepositoryLinkId,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}