using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-autoscaling", "describe-scheduled-actions")]
public record AwsApplicationAutoscalingDescribeScheduledActionsOptions(
[property: CommandSwitch("--service-namespace")] string ServiceNamespace
) : AwsOptions
{
    [CommandSwitch("--scheduled-action-names")]
    public string[]? ScheduledActionNames { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--scalable-dimension")]
    public string? ScalableDimension { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}