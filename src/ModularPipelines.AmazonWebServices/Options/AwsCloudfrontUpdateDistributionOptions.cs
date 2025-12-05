using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-distribution")]
public record AwsCloudfrontUpdateDistributionOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--distribution-config")]
    public string? DistributionConfig { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--default-root-object")]
    public string? DefaultRootObject { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}