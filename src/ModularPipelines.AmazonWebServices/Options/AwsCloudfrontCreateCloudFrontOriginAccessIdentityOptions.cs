using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-cloud-front-origin-access-identity")]
public record AwsCloudfrontCreateCloudFrontOriginAccessIdentityOptions(
[property: CommandSwitch("--cloud-front-origin-access-identity-config")] string CloudFrontOriginAccessIdentityConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}