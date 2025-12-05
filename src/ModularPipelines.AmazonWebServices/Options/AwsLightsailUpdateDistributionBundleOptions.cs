using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-distribution-bundle")]
public record AwsLightsailUpdateDistributionBundleOptions : AwsOptions
{
    [CliOption("--distribution-name")]
    public string? DistributionName { get; set; }

    [CliOption("--bundle-id")]
    public string? BundleId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}