using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "describe-instance-refreshes")]
public record AwsAutoscalingDescribeInstanceRefreshesOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--instance-refresh-ids")]
    public string[]? InstanceRefreshIds { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-records")]
    public int? MaxRecords { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}