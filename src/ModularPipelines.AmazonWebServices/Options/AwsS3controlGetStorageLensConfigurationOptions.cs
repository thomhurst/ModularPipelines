using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "get-storage-lens-configuration")]
public record AwsS3controlGetStorageLensConfigurationOptions(
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}