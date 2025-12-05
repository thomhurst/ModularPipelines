using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "describe-image-replication-status")]
public record AwsEcrDescribeImageReplicationStatusOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--image-id")] string ImageId
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}