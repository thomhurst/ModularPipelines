using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-report-plan")]
public record AwsBackupCreateReportPlanOptions(
[property: CommandSwitch("--report-plan-name")] string ReportPlanName,
[property: CommandSwitch("--report-delivery-channel")] string ReportDeliveryChannel,
[property: CommandSwitch("--report-setting")] string ReportSetting
) : AwsOptions
{
    [CommandSwitch("--report-plan-description")]
    public string? ReportPlanDescription { get; set; }

    [CommandSwitch("--report-plan-tags")]
    public IEnumerable<KeyValue>? ReportPlanTags { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}