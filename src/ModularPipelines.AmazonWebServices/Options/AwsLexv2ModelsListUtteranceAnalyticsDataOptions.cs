using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-utterance-analytics-data")]
public record AwsLexv2ModelsListUtteranceAnalyticsDataOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--start-date-time")] long StartDateTime,
[property: CommandSwitch("--end-date-time")] long EndDateTime
) : AwsOptions
{
    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}