using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-distribution-latest-cache-reset")]
public record AwsLightsailGetDistributionLatestCacheResetOptions : AwsOptions
{
    [CliOption("--distribution-name")]
    public string? DistributionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}