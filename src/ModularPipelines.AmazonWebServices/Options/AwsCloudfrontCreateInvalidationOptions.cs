using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-invalidation")]
public record AwsCloudfrontCreateInvalidationOptions(
[property: CliOption("--distribution-id")] string DistributionId
) : AwsOptions
{
    [CliOption("--invalidation-batch")]
    public string? InvalidationBatch { get; set; }

    [CliOption("--paths")]
    public string? Paths { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}