using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "delete-storage-lens-group")]
public record AwsS3controlDeleteStorageLensGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}