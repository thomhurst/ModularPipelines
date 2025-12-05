using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-streaming-distribution")]
public record AwsCloudfrontCreateStreamingDistributionOptions(
[property: CliOption("--streaming-distribution-config")] string StreamingDistributionConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}