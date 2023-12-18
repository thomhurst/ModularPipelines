using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "start-stop-schedule", "create")]
public record AzSqlMiStartStopScheduleCreateOptions(
[property: CommandSwitch("--managed-instance")] string ManagedInstance,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--schedule-list")]
    public string? ScheduleList { get; set; }

    [CommandSwitch("--timezone-id")]
    public string? TimezoneId { get; set; }
}