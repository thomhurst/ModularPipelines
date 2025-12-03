using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "list-repository-sync-definitions")]
public record AwsCodestarConnectionsListRepositorySyncDefinitionsOptions(
[property: CliOption("--repository-link-id")] string RepositoryLinkId,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}