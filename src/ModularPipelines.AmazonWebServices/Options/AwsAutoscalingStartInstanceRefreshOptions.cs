using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "start-instance-refresh")]
public record AwsAutoscalingStartInstanceRefreshOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandSwitch("--desired-configuration")]
    public string? DesiredConfiguration { get; set; }

    [CommandSwitch("--preferences")]
    public string? Preferences { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}