using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "data-history", "connection", "create", "adx")]
public record AzDtDataHistoryConnectionCreateAdxOptions(
[property: CommandSwitch("--adx-cluster-name")] string AdxClusterName,
[property: CommandSwitch("--adx-database-name")] string AdxDatabaseName,
[property: CommandSwitch("--cn")] string Cn,
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--eh")] string Eh,
[property: CommandSwitch("--ehn")] string Ehn
) : AzOptions
{
    [CommandSwitch("--adx-property-events-table")]
    public string? AdxPropertyEventsTable { get; set; }

    [BooleanCommandSwitch("--adx-record-removals")]
    public bool? AdxRecordRemovals { get; set; }

    [CommandSwitch("--adx-relationship-events-table")]
    public string? AdxRelationshipEventsTable { get; set; }

    [CommandSwitch("--adx-resource-group")]
    public string? AdxResourceGroup { get; set; }

    [CommandSwitch("--adx-subscription")]
    public string? AdxSubscription { get; set; }

    [CommandSwitch("--adx-table-name")]
    public string? AdxTableName { get; set; }

    [CommandSwitch("--adx-twin-events-table")]
    public string? AdxTwinEventsTable { get; set; }

    [CommandSwitch("--ehc")]
    public string? Ehc { get; set; }

    [CommandSwitch("--ehg")]
    public string? Ehg { get; set; }

    [CommandSwitch("--ehs")]
    public string? Ehs { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}