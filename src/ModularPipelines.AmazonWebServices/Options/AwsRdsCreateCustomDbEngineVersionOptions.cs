using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-custom-db-engine-version")]
public record AwsRdsCreateCustomDbEngineVersionOptions(
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CommandSwitch("--database-installation-files-s3-bucket-name")]
    public string? DatabaseInstallationFilesS3BucketName { get; set; }

    [CommandSwitch("--database-installation-files-s3-prefix")]
    public string? DatabaseInstallationFilesS3Prefix { get; set; }

    [CommandSwitch("--image-id")]
    public string? ImageId { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--manifest")]
    public string? Manifest { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--source-custom-db-engine-version-identifier")]
    public string? SourceCustomDbEngineVersionIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}