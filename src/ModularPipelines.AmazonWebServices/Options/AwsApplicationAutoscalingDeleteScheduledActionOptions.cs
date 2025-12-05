using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-autoscaling", "delete-scheduled-action")]
public record AwsApplicationAutoscalingDeleteScheduledActionOptions(
[property: CliOption("--service-namespace")] string ServiceNamespace,
[property: CliOption("--scheduled-action-name")] string ScheduledActionName,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}