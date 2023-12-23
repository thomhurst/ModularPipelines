using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "describe-policies")]
public record AwsAutoscalingDescribePoliciesOptions : AwsOptions
{
    [CommandSwitch("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CommandSwitch("--policy-names")]
    public string[]? PolicyNames { get; set; }

    [CommandSwitch("--policy-types")]
    public string[]? PolicyTypes { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}