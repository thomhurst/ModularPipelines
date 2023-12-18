using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "update")]
public record AzConnectedmachineUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--agent-upgrade")]
    public string? AgentUpgrade { get; set; }

    [CommandSwitch("--client-public-key")]
    public string? ClientPublicKey { get; set; }

    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--extensions")]
    public string? Extensions { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--license-profile")]
    public string? LicenseProfile { get; set; }

    [CommandSwitch("--location-data")]
    public string? LocationData { get; set; }

    [CommandSwitch("--machine-name")]
    public string? MachineName { get; set; }

    [CommandSwitch("--mssql-discovered")]
    public string? MssqlDiscovered { get; set; }

    [CommandSwitch("--os-profile")]
    public string? OsProfile { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--parent-cluster-id")]
    public string? ParentClusterId { get; set; }

    [CommandSwitch("--private-link-scope-resource-id")]
    public string? PrivateLinkScopeResourceId { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-statuses")]
    public string? ServiceStatuses { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}