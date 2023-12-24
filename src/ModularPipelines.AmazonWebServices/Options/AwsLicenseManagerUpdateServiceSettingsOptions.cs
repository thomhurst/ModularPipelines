using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "update-service-settings")]
public record AwsLicenseManagerUpdateServiceSettingsOptions : AwsOptions
{
    [CommandSwitch("--s3-bucket-arn")]
    public string? S3BucketArn { get; set; }

    [CommandSwitch("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CommandSwitch("--organization-configuration")]
    public string? OrganizationConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}