using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "delete-access-grants-location")]
public record AwsS3controlDeleteAccessGrantsLocationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--access-grants-location-id")] string AccessGrantsLocationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}