using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "search-associated-transcripts")]
public record AwsLexv2ModelsSearchAssociatedTranscriptsOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--bot-version")] string BotVersion,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--bot-recommendation-id")] string BotRecommendationId,
[property: CommandSwitch("--filters")] string[] Filters
) : AwsOptions
{
    [CommandSwitch("--search-order")]
    public string? SearchOrder { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-index")]
    public int? NextIndex { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}