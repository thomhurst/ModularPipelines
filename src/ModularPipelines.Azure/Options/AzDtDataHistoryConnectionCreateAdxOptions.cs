using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "data-history", "connection", "create", "adx")]
public record AzDtDataHistoryConnectionCreateAdxOptions(
[property: CliOption("--adx-cluster-name")] string AdxClusterName,
[property: CliOption("--adx-database-name")] string AdxDatabaseName,
[property: CliOption("--cn")] string Cn,
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--eh")] string Eh,
[property: CliOption("--ehn")] string Ehn
) : AzOptions
{
    [CliOption("--adx-property-events-table")]
    public string? AdxPropertyEventsTable { get; set; }

    [CliFlag("--adx-record-removals")]
    public bool? AdxRecordRemovals { get; set; }

    [CliOption("--adx-relationship-events-table")]
    public string? AdxRelationshipEventsTable { get; set; }

    [CliOption("--adx-resource-group")]
    public string? AdxResourceGroup { get; set; }

    [CliOption("--adx-subscription")]
    public string? AdxSubscription { get; set; }

    [CliOption("--adx-twin-events-table")]
    public string? AdxTwinEventsTable { get; set; }

    [CliOption("--ehc")]
    public string? Ehc { get; set; }

    [CliOption("--ehg")]
    public string? Ehg { get; set; }

    [CliOption("--ehs")]
    public string? Ehs { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}