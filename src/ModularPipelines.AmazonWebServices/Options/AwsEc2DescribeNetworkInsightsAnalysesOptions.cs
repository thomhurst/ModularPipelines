using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-network-insights-analyses")]
public record AwsEc2DescribeNetworkInsightsAnalysesOptions : AwsOptions
{
    [CliOption("--network-insights-analysis-ids")]
    public string[]? NetworkInsightsAnalysisIds { get; set; }

    [CliOption("--network-insights-path-id")]
    public string? NetworkInsightsPathId { get; set; }

    [CliOption("--analysis-start-time")]
    public long? AnalysisStartTime { get; set; }

    [CliOption("--analysis-end-time")]
    public long? AnalysisEndTime { get; set; }

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