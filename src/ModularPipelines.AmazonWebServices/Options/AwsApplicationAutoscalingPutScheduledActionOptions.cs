using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-autoscaling", "put-scheduled-action")]
public record AwsApplicationAutoscalingPutScheduledActionOptions(
[property: CliOption("--service-namespace")] string ServiceNamespace,
[property: CliOption("--scheduled-action-name")] string ScheduledActionName,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--scalable-target-action")]
    public string? ScalableTargetAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}