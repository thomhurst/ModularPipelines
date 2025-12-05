using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-custom-db-engine-version")]
public record AwsRdsCreateCustomDbEngineVersionOptions(
[property: CliOption("--engine")] string Engine,
[property: CliOption("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CliOption("--database-installation-files-s3-bucket-name")]
    public string? DatabaseInstallationFilesS3BucketName { get; set; }

    [CliOption("--database-installation-files-s3-prefix")]
    public string? DatabaseInstallationFilesS3Prefix { get; set; }

    [CliOption("--image-id")]
    public string? ImageId { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--manifest")]
    public string? Manifest { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--source-custom-db-engine-version-identifier")]
    public string? SourceCustomDbEngineVersionIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}