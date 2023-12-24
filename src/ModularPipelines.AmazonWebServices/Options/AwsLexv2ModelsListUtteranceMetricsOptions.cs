using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-utterance-metrics")]
public record AwsLexv2ModelsListUtteranceMetricsOptions(
[property: CommandSwitch("--bot-id")] string BotId,
[property: CommandSwitch("--start-date-time")] long StartDateTime,
[property: CommandSwitch("--end-date-time")] long EndDateTime,
[property: CommandSwitch("--metrics")] string[] Metrics
) : AwsOptions
{
    [CommandSwitch("--bin-by")]
    public string[]? BinBy { get; set; }

    [CommandSwitch("--group-by")]
    public string[]? GroupBy { get; set; }

    [CommandSwitch("--attributes")]
    public string[]? Attributes { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}