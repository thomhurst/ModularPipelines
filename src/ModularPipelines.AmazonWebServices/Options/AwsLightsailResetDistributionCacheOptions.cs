using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "reset-distribution-cache")]
public record AwsLightsailResetDistributionCacheOptions : AwsOptions
{
    [CliOption("--distribution-name")]
    public string? DistributionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}