using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "list-session-analytics-data")]
public record AwsLexv2ModelsListSessionAnalyticsDataOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--start-date-time")] long StartDateTime,
[property: CliOption("--end-date-time")] long EndDateTime
) : AwsOptions
{
    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}