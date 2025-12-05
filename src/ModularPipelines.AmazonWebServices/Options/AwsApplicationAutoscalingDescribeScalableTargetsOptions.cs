using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-autoscaling", "describe-scalable-targets")]
public record AwsApplicationAutoscalingDescribeScalableTargetsOptions(
[property: CliOption("--service-namespace")] string ServiceNamespace
) : AwsOptions
{
    [CliOption("--resource-ids")]
    public string[]? ResourceIds { get; set; }

    [CliOption("--scalable-dimension")]
    public string? ScalableDimension { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}