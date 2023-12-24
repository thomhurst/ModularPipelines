using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-autoscaling", "put-scheduled-action")]
public record AwsApplicationAutoscalingPutScheduledActionOptions(
[property: CommandSwitch("--service-namespace")] string ServiceNamespace,
[property: CommandSwitch("--scheduled-action-name")] string ScheduledActionName,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--timezone")]
    public string? Timezone { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--scalable-target-action")]
    public string? ScalableTargetAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}