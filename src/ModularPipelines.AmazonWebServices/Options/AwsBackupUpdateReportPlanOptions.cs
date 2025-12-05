using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-report-plan")]
public record AwsBackupUpdateReportPlanOptions(
[property: CliOption("--report-plan-name")] string ReportPlanName
) : AwsOptions
{
    [CliOption("--report-plan-description")]
    public string? ReportPlanDescription { get; set; }

    [CliOption("--report-delivery-channel")]
    public string? ReportDeliveryChannel { get; set; }

    [CliOption("--report-setting")]
    public string? ReportSetting { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}