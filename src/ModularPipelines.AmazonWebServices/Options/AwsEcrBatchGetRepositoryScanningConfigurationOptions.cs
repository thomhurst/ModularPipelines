using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "batch-get-repository-scanning-configuration")]
public record AwsEcrBatchGetRepositoryScanningConfigurationOptions(
[property: CliOption("--repository-names")] string[] RepositoryNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}