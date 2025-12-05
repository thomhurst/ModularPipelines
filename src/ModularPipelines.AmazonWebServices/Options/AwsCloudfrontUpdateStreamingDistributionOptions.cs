using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-streaming-distribution")]
public record AwsCloudfrontUpdateStreamingDistributionOptions(
[property: CliOption("--streaming-distribution-config")] string StreamingDistributionConfig,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}