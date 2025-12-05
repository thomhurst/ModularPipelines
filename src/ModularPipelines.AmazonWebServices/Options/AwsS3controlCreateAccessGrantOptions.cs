using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-access-grant")]
public record AwsS3controlCreateAccessGrantOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--access-grants-location-id")] string AccessGrantsLocationId,
[property: CliOption("--grantee")] string Grantee,
[property: CliOption("--permission")] string Permission
) : AwsOptions
{
    [CliOption("--access-grants-location-configuration")]
    public string? AccessGrantsLocationConfiguration { get; set; }

    [CliOption("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CliOption("--s3-prefix-type")]
    public string? S3PrefixType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}