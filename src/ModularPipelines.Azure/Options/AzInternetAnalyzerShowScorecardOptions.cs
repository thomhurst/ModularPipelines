using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internet-analyzer", "show-scorecard")]
public record AzInternetAnalyzerShowScorecardOptions(
[property: CommandSwitch("--aggregation-interval")] string AggregationInterval,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--test-name")] string TestName
) : AzOptions
{
    [CommandSwitch("--country")]
    public int? Country { get; set; }

    [CommandSwitch("--end-date-time-utc")]
    public string? EndDateTimeUtc { get; set; }
}