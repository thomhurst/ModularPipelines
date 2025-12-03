using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "search-associated-transcripts")]
public record AwsLexv2ModelsSearchAssociatedTranscriptsOptions(
[property: CliOption("--bot-id")] string BotId,
[property: CliOption("--bot-version")] string BotVersion,
[property: CliOption("--locale-id")] string LocaleId,
[property: CliOption("--bot-recommendation-id")] string BotRecommendationId,
[property: CliOption("--filters")] string[] Filters
) : AwsOptions
{
    [CliOption("--search-order")]
    public string? SearchOrder { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-index")]
    public int? NextIndex { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}