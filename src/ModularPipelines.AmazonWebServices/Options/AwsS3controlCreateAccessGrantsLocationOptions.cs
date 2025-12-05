using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-access-grants-location")]
public record AwsS3controlCreateAccessGrantsLocationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--location-scope")] string LocationScope,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}