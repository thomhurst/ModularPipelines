using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-distribution-bundle")]
public record AwsLightsailUpdateDistributionBundleOptions : AwsOptions
{
    [CommandSwitch("--distribution-name")]
    public string? DistributionName { get; set; }

    [CommandSwitch("--bundle-id")]
    public string? BundleId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}