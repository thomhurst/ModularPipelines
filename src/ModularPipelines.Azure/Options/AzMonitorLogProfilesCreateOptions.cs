using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-profiles", "create")]
public record AzMonitorLogProfilesCreateOptions(
[property: CommandSwitch("--categories")] string Categories,
[property: CommandSwitch("--days")] int Days,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--locations")] string Locations,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--service-bus-rule-id")]
    public string? ServiceBusRuleId { get; set; }

    [CommandSwitch("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}