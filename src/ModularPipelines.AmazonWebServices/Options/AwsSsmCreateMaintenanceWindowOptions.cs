using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "create-maintenance-window")]
public record AwsSsmCreateMaintenanceWindowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--duration")] int Duration,
[property: CliOption("--cutoff")] int Cutoff
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--schedule-timezone")]
    public string? ScheduleTimezone { get; set; }

    [CliOption("--schedule-offset")]
    public int? ScheduleOffset { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}