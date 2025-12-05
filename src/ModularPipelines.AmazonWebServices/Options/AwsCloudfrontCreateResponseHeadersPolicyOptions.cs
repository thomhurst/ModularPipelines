using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-response-headers-policy")]
public record AwsCloudfrontCreateResponseHeadersPolicyOptions(
[property: CliOption("--response-headers-policy-config")] string ResponseHeadersPolicyConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}