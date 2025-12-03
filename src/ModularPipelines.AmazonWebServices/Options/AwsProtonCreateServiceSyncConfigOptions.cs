using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-service-sync-config")]
public record AwsProtonCreateServiceSyncConfigOptions(
[property: CliOption("--branch")] string Branch,
[property: CliOption("--file-path")] string FilePath,
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--repository-provider")] string RepositoryProvider,
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}