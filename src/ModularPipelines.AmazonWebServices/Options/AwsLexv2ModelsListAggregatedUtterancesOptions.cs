using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-aggregated-utterances")]
public record AwsLexv2ModelsListAggregatedUtterancesOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--locale-id")] string LocaleId,
[property: CommandSwitch("--aggregation-duration")] string AggregationDuration
) : AwsOptions
{
    [CommandSwitch("--bot-alias-id")]
    public string? BotAliasId { get; set; }

    [CommandSwitch("--bot-version")]
    public string? BotVersion { get; set; }

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