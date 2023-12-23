using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-distribution-latest-cache-reset")]
public record AwsLightsailGetDistributionLatestCacheResetOptions : AwsOptions
{
    [CommandSwitch("--distribution-name")]
    public string? DistributionName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}