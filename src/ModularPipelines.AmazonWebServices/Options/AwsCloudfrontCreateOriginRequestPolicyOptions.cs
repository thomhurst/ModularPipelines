using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-origin-request-policy")]
public record AwsCloudfrontCreateOriginRequestPolicyOptions(
[property: CliOption("--origin-request-policy-config")] string OriginRequestPolicyConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}