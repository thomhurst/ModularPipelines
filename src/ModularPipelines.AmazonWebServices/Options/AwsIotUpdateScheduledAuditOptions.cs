using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-scheduled-audit")]
public record AwsIotUpdateScheduledAuditOptions(
[property: CliOption("--scheduled-audit-name")] string ScheduledAuditName
) : AwsOptions
{
    [CliOption("--frequency")]
    public string? Frequency { get; set; }

    [CliOption("--day-of-month")]
    public string? DayOfMonth { get; set; }

    [CliOption("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CliOption("--target-check-names")]
    public string[]? TargetCheckNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}