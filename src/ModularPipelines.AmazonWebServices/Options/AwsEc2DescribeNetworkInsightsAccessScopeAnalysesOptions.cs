using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-network-insights-access-scope-analyses")]
public record AwsEc2DescribeNetworkInsightsAccessScopeAnalysesOptions : AwsOptions
{
    [CliOption("--network-insights-access-scope-analysis-ids")]
    public string[]? NetworkInsightsAccessScopeAnalysisIds { get; set; }

    [CliOption("--network-insights-access-scope-id")]
    public string? NetworkInsightsAccessScopeId { get; set; }

    [CliOption("--analysis-start-time-begin")]
    public long? AnalysisStartTimeBegin { get; set; }

    [CliOption("--analysis-start-time-end")]
    public long? AnalysisStartTimeEnd { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}