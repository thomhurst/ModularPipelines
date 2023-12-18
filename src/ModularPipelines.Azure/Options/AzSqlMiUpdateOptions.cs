using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "update")]
public record AzSqlMiUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CommandSwitch("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--instance-pool-name")]
    public string? InstancePoolName { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pid")]
    public string? Pid { get; set; }

    [CommandSwitch("--proxy-override")]
    public string? ProxyOverride { get; set; }

    [BooleanCommandSwitch("--public-data-endpoint-enabled")]
    public bool? PublicDataEndpointEnabled { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-principal-type")]
    public string? ServicePrincipalType { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--storage")]
    public string? Storage { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}

