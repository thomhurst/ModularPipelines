using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "list-intent-metrics")]
public record AwsLexv2ModelsListIntentMetricsOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--start-date-time")] long StartDateTime,
[property: CliOption("--end-date-time")] long EndDateTime,
[property: CliOption("--metrics")] string[] Metrics
) : AwsOptions
{
    [CliOption("--bin-by")]
    public string[]? BinBy { get; set; }

    [CliOption("--group-by")]
    public string[]? GroupBy { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}