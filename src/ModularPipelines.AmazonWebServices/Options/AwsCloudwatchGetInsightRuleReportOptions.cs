using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "get-insight-rule-report")]
public record AwsCloudwatchGetInsightRuleReportOptions(
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--period")] int Period
) : AwsOptions
{
    [CommandSwitch("--max-contributor-count")]
    public int? MaxContributorCount { get; set; }

    [CommandSwitch("--metrics")]
    public string[]? Metrics { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}