using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-campaign-date-range-kpi")]
public record AwsPinpointGetCampaignDateRangeKpiOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--campaign-id")] string CampaignId,
[property: CommandSwitch("--kpi-name")] string KpiName
) : AwsOptions
{
    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}