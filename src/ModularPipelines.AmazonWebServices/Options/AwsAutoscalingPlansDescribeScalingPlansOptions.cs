using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling-plans", "describe-scaling-plans")]
public record AwsAutoscalingPlansDescribeScalingPlansOptions : AwsOptions
{
    [CommandSwitch("--scaling-plan-names")]
    public string[]? ScalingPlanNames { get; set; }

    [CommandSwitch("--scaling-plan-version")]
    public long? ScalingPlanVersion { get; set; }

    [CommandSwitch("--application-sources")]
    public string[]? ApplicationSources { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}