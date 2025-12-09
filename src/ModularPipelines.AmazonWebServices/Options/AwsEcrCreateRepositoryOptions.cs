using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "create-repository")]
public record AwsEcrCreateRepositoryOptions(
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--image-tag-mutability")]
    public string? ImageTagMutability { get; set; }

    [CliOption("--image-scanning-configuration")]
    public string? ImageScanningConfiguration { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}