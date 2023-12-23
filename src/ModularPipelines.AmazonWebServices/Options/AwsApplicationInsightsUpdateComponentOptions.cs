using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "update-component")]
public record AwsApplicationInsightsUpdateComponentOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--component-name")] string ComponentName
) : AwsOptions
{
    [CommandSwitch("--new-component-name")]
    public string? NewComponentName { get; set; }

    [CommandSwitch("--resource-list")]
    public string[]? ResourceList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}