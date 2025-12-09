using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "update-access-grants-location")]
public record AwsS3controlUpdateAccessGrantsLocationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--access-grants-location-id")] string AccessGrantsLocationId,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}