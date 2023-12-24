using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "list-anomalies-for-insight")]
public record AwsDevopsGuruListAnomaliesForInsightOptions(
[property: CommandSwitch("--insight-id")] string InsightId
) : AwsOptions
{
    [CommandSwitch("--start-time-range")]
    public string? StartTimeRange { get; set; }

    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}