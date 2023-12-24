using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-distribution")]
public record AwsLightsailUpdateDistributionOptions(
[property: CommandSwitch("--distribution-name")] string DistributionName
) : AwsOptions
{
    [CommandSwitch("--origin")]
    public string? Origin { get; set; }

    [CommandSwitch("--default-cache-behavior")]
    public string? DefaultCacheBehavior { get; set; }

    [CommandSwitch("--cache-behavior-settings")]
    public string? CacheBehaviorSettings { get; set; }

    [CommandSwitch("--cache-behaviors")]
    public string[]? CacheBehaviors { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}