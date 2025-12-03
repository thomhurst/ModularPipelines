using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-response-headers-policy")]
public record AwsCloudfrontUpdateResponseHeadersPolicyOptions(
[property: CliOption("--response-headers-policy-config")] string ResponseHeadersPolicyConfig,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}