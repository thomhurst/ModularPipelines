using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("costmanagement", "export", "create")]
public record AzCostmanagementExportCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--storage-account-id")] int StorageAccountId,
[property: CliOption("--storage-container")] string StorageContainer,
[property: CliOption("--timeframe")] string Timeframe
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

    [CliOption("--storage-directory")]
    public string? StorageDirectory { get; set; }

    [CliOption("--time-period")]
    public string? TimePeriod { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}