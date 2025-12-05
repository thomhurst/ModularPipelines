using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "start-instance-refresh")]
public record AwsAutoscalingStartInstanceRefreshOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CliOption("--strategy")]
    public string? Strategy { get; set; }

    [CliOption("--desired-configuration")]
    public string? DesiredConfiguration { get; set; }

    [CliOption("--preferences")]
    public string? Preferences { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}