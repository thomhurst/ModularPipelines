using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-blob")]
public record AwsCodecommitGetBlobOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--blob-id")] string BlobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}