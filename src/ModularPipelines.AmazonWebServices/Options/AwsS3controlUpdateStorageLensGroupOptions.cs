using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "update-storage-lens-group")]
public record AwsS3controlUpdateStorageLensGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--storage-lens-group")] string StorageLensGroup
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}