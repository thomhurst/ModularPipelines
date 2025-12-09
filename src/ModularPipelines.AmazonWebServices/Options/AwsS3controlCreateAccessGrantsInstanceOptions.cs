using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-access-grants-instance")]
public record AwsS3controlCreateAccessGrantsInstanceOptions(
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--identity-center-arn")]
    public string? IdentityCenterArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}