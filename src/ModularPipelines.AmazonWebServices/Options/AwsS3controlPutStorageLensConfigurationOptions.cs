using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-storage-lens-configuration")]
public record AwsS3controlPutStorageLensConfigurationOptions(
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--storage-lens-configuration")] string StorageLensConfiguration
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}