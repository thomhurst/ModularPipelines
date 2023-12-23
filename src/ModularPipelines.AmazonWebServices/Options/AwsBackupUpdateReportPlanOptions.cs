using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-report-plan")]
public record AwsBackupUpdateReportPlanOptions(
[property: CommandSwitch("--report-plan-name")] string ReportPlanName
) : AwsOptions
{
    [CommandSwitch("--report-plan-description")]
    public string? ReportPlanDescription { get; set; }

    [CommandSwitch("--report-delivery-channel")]
    public string? ReportDeliveryChannel { get; set; }

    [CommandSwitch("--report-setting")]
    public string? ReportSetting { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}