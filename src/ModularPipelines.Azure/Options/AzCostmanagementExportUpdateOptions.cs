using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("costmanagement", "export", "update")]
public record AzCostmanagementExportUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--dataset-configuration")]
    public string? DatasetConfiguration { get; set; }

    [CliOption("--recurrence")]
    public string? Recurrence { get; set; }

    [CliOption("--recurrence-period")]
    public string? RecurrencePeriod { get; set; }

    [CliOption("--schedule-status")]
    public string? ScheduleStatus { get; set; }

    [CliOption("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CliOption("--storage-container")]
    public string? StorageContainer { get; set; }

    [CliOption("--storage-directory")]
    public string? StorageDirectory { get; set; }

    [CliOption("--time-period")]
    public string? TimePeriod { get; set; }

    [CliOption("--timeframe")]
    public string? Timeframe { get; set; }
}