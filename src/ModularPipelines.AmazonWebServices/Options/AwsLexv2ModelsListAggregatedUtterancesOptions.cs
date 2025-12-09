using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "list-aggregated-utterances")]
public record AwsLexv2ModelsListAggregatedUtterancesOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--aggregation-duration")] string AggregationDuration
) : AwsOptions
{
    [CliOption("--bot-alias-id")]
    public string? BotAliasId { get; set; }

    [CliOption("--bot-version")]
    public string? BotVersion { get; set; }

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