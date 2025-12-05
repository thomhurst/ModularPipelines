using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "update-service-settings")]
public record AwsLicenseManagerUpdateServiceSettingsOptions : AwsOptions
{
    [CliOption("--s3-bucket-arn")]
    public string? S3BucketArn { get; set; }

    [CliOption("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CliOption("--organization-configuration")]
    public string? OrganizationConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}