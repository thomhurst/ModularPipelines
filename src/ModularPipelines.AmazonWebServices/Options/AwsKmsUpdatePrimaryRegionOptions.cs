using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "update-primary-region")]
public record AwsKmsUpdatePrimaryRegionOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--primary-region")] string PrimaryRegion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}