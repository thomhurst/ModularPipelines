using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "get-repository-sync-status")]
public record AwsProtonGetRepositorySyncStatusOptions(
[property: CliOption("--branch")] string Branch,
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--repository-provider")] string RepositoryProvider,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}