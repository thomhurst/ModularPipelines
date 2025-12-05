using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("internet-analyzer", "show-scorecard")]
public record AzInternetAnalyzerShowScorecardOptions(
[property: CliOption("--aggregation-interval")] string AggregationInterval,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--test-name")] string TestName
) : AzOptions
{
    [CliOption("--country")]
    public int? Country { get; set; }

    [CliOption("--end-date-time-utc")]
    public string? EndDateTimeUtc { get; set; }
}