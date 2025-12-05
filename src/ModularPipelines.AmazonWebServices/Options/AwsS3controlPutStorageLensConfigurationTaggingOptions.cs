using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-storage-lens-configuration-tagging")]
public record AwsS3controlPutStorageLensConfigurationTaggingOptions(
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}