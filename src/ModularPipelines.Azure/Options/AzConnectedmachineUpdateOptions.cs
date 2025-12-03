using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedmachine", "update")]
public record AzConnectedmachineUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--agent-upgrade")]
    public string? AgentUpgrade { get; set; }

    [CliOption("--client-public-key")]
    public string? ClientPublicKey { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--extensions")]
    public string? Extensions { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--license-profile")]
    public string? LicenseProfile { get; set; }

    [CliOption("--location-data")]
    public string? LocationData { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--mssql-discovered")]
    public string? MssqlDiscovered { get; set; }

    [CliOption("--os-profile")]
    public string? OsProfile { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--parent-cluster-id")]
    public string? ParentClusterId { get; set; }

    [CliOption("--private-link-scope-resource-id")]
    public string? PrivateLinkScopeResourceId { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-statuses")]
    public string? ServiceStatuses { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}