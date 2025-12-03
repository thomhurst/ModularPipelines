using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-distribution")]
public record AwsLightsailCreateDistributionOptions(
[property: CliOption("--distribution-name")] string DistributionName,
[property: CliOption("--origin")] string Origin,
[property: CliOption("--default-cache-behavior")] string DefaultCacheBehavior,
[property: CliOption("--bundle-id")] string BundleId
) : AwsOptions
{
    [CliOption("--cache-behavior-settings")]
    public string? CacheBehaviorSettings { get; set; }

    [CliOption("--cache-behaviors")]
    public string[]? CacheBehaviors { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}