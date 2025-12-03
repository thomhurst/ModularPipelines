using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-distribution-with-staging-config")]
public record AwsCloudfrontUpdateDistributionWithStagingConfigOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--staging-distribution-id")]
    public string? StagingDistributionId { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}