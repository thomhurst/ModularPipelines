using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-profiles", "create")]
public record AzMonitorLogProfilesCreateOptions(
[property: CliOption("--categories")] string Categories,
[property: CliOption("--days")] int Days,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--location")] string Location,
[property: CliOption("--locations")] string Locations,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--service-bus-rule-id")]
    public string? ServiceBusRuleId { get; set; }

    [CliOption("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}