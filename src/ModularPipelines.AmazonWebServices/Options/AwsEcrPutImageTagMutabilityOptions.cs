using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "put-image-tag-mutability")]
public record AwsEcrPutImageTagMutabilityOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--image-tag-mutability")] string ImageTagMutability
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}