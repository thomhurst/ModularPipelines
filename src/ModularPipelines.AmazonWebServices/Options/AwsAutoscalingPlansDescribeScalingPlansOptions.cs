using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling-plans", "describe-scaling-plans")]
public record AwsAutoscalingPlansDescribeScalingPlansOptions : AwsOptions
{
    [CliOption("--scaling-plan-names")]
    public string[]? ScalingPlanNames { get; set; }

    [CliOption("--scaling-plan-version")]
    public long? ScalingPlanVersion { get; set; }

    [CliOption("--application-sources")]
    public string[]? ApplicationSources { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}