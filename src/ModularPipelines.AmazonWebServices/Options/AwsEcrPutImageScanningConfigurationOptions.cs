using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "put-image-scanning-configuration")]
public record AwsEcrPutImageScanningConfigurationOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--image-scanning-configuration")] string ImageScanningConfiguration
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}