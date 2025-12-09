using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "list-repository-sync-definitions")]
public record AwsProtonListRepositorySyncDefinitionsOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--repository-provider")] string RepositoryProvider,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}