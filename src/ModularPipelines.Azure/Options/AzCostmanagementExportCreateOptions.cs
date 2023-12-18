using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("costmanagement", "export", "create")]
public record AzCostmanagementExportCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--storage-account-id")] int StorageAccountId,
[property: CommandSwitch("--storage-container")] string StorageContainer,
[property: CommandSwitch("--timeframe")] string Timeframe
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

    [CommandSwitch("--storage-directory")]
    public string? StorageDirectory { get; set; }

    [CommandSwitch("--time-period")]
    public string? TimePeriod { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}