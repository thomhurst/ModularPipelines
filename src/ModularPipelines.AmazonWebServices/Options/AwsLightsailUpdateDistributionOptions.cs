using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-distribution")]
public record AwsLightsailUpdateDistributionOptions(
[property: CliOption("--distribution-name")] string DistributionName
) : AwsOptions
{
    [CliOption("--origin")]
    public string? Origin { get; set; }

    [CliOption("--default-cache-behavior")]
    public string? DefaultCacheBehavior { get; set; }

    [CliOption("--cache-behavior-settings")]
    public string? CacheBehaviorSettings { get; set; }

    [CliOption("--cache-behaviors")]
    public string[]? CacheBehaviors { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}