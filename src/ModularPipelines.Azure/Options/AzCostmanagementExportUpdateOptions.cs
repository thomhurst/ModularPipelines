using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("costmanagement", "export", "update")]
public record AzCostmanagementExportUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--dataset-configuration")]
    public string? DatasetConfiguration { get; set; }

    [CommandSwitch("--recurrence")]
    public string? Recurrence { get; set; }

    [CommandSwitch("--recurrence-period")]
    public string? RecurrencePeriod { get; set; }

    [CommandSwitch("--schedule-status")]
    public string? ScheduleStatus { get; set; }

    [CommandSwitch("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CommandSwitch("--storage-container")]
    public string? StorageContainer { get; set; }

    [CommandSwitch("--storage-directory")]
    public string? StorageDirectory { get; set; }

    [CommandSwitch("--time-period")]
    public string? TimePeriod { get; set; }

    [CommandSwitch("--timeframe")]
    public string? Timeframe { get; set; }
}