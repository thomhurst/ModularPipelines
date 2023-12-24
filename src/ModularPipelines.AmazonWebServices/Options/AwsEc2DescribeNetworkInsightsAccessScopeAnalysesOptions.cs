using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-network-insights-access-scope-analyses")]
public record AwsEc2DescribeNetworkInsightsAccessScopeAnalysesOptions : AwsOptions
{
    [CommandSwitch("--network-insights-access-scope-analysis-ids")]
    public string[]? NetworkInsightsAccessScopeAnalysisIds { get; set; }

    [CommandSwitch("--network-insights-access-scope-id")]
    public string? NetworkInsightsAccessScopeId { get; set; }

    [CommandSwitch("--analysis-start-time-begin")]
    public long? AnalysisStartTimeBegin { get; set; }

    [CommandSwitch("--analysis-start-time-end")]
    public long? AnalysisStartTimeEnd { get; set; }

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