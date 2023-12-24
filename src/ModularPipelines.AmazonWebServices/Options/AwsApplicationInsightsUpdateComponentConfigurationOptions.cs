using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "update-component-configuration")]
public record AwsApplicationInsightsUpdateComponentConfigurationOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--component-name")] string ComponentName
) : AwsOptions
{
    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--component-configuration")]
    public string? ComponentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}