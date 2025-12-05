using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-report-plan")]
public record AwsBackupCreateReportPlanOptions(
[property: CliOption("--report-plan-name")] string ReportPlanName,
[property: CliOption("--report-delivery-channel")] string ReportDeliveryChannel,
[property: CliOption("--report-setting")] string ReportSetting
) : AwsOptions
{
    [CliOption("--report-plan-description")]
    public string? ReportPlanDescription { get; set; }

    [CliOption("--report-plan-tags")]
    public IEnumerable<KeyValue>? ReportPlanTags { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}