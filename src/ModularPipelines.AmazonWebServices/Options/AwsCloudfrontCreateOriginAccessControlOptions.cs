using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-origin-access-control")]
public record AwsCloudfrontCreateOriginAccessControlOptions(
[property: CliOption("--origin-access-control-config")] string OriginAccessControlConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}