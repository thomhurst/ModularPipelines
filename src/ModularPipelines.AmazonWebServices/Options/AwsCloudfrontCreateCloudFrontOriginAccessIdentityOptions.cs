using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-cloud-front-origin-access-identity")]
public record AwsCloudfrontCreateCloudFrontOriginAccessIdentityOptions(
[property: CliOption("--cloud-front-origin-access-identity-config")] string CloudFrontOriginAccessIdentityConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}