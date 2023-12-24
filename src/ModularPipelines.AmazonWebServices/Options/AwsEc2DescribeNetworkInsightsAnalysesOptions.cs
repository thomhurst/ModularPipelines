using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-network-insights-analyses")]
public record AwsEc2DescribeNetworkInsightsAnalysesOptions : AwsOptions
{
    [CommandSwitch("--network-insights-analysis-ids")]
    public string[]? NetworkInsightsAnalysisIds { get; set; }

    [CommandSwitch("--network-insights-path-id")]
    public string? NetworkInsightsPathId { get; set; }

    [CommandSwitch("--analysis-start-time")]
    public long? AnalysisStartTime { get; set; }

    [CommandSwitch("--analysis-end-time")]
    public long? AnalysisEndTime { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}