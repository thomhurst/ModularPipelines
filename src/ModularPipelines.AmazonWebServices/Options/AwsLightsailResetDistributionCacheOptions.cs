using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "reset-distribution-cache")]
public record AwsLightsailResetDistributionCacheOptions : AwsOptions
{
    [CommandSwitch("--distribution-name")]
    public string? DistributionName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}