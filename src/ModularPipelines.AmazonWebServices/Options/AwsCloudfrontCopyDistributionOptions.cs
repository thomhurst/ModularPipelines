using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "copy-distribution")]
public record AwsCloudfrontCopyDistributionOptions(
[property: CliOption("--primary-distribution-id")] string PrimaryDistributionId,
[property: CliOption("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}