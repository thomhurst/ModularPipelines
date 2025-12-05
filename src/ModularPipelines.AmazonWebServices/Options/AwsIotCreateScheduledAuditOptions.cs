using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-scheduled-audit")]
public record AwsIotCreateScheduledAuditOptions(
[property: CliOption("--frequency")] string Frequency,
[property: CliOption("--target-check-names")] string[] TargetCheckNames,
[property: CliOption("--scheduled-audit-name")] string ScheduledAuditName
) : AwsOptions
{
    [CliOption("--day-of-month")]
    public string? DayOfMonth { get; set; }

    [CliOption("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}