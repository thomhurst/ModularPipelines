using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi", "start-stop-schedule", "create")]
public record AzSqlMiStartStopScheduleCreateOptions(
[property: CliOption("--managed-instance")] string ManagedInstance,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schedule-list")]
    public string? ScheduleList { get; set; }

    [CliOption("--timezone-id")]
    public string? TimezoneId { get; set; }
}