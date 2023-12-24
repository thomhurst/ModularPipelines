using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rum", "update-app-monitor")]
public record AwsRumUpdateAppMonitorOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--app-monitor-configuration")]
    public string? AppMonitorConfiguration { get; set; }

    [CommandSwitch("--custom-events")]
    public string? CustomEvents { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}