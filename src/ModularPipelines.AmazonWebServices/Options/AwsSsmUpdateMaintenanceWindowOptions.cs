using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-maintenance-window")]
public record AwsSsmUpdateMaintenanceWindowOptions(
[property: CliOption("--window-id")] string WindowId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--schedule-timezone")]
    public string? ScheduleTimezone { get; set; }

    [CliOption("--schedule-offset")]
    public int? ScheduleOffset { get; set; }

    [CliOption("--duration")]
    public int? Duration { get; set; }

    [CliOption("--cutoff")]
    public int? Cutoff { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}