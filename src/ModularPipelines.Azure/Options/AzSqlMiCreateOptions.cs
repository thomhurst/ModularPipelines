using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "create")]
public record AzSqlMiCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-user")]
    public string? AdminUser { get; set; }

    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CommandSwitch("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [BooleanCommandSwitch("--enable-ad-only-auth")]
    public bool? EnableAdOnlyAuth { get; set; }

    [CommandSwitch("--external-admin-name")]
    public string? ExternalAdminName { get; set; }

    [CommandSwitch("--external-admin-principal-type")]
    public string? ExternalAdminPrincipalType { get; set; }

    [CommandSwitch("--external-admin-sid")]
    public string? ExternalAdminSid { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--instance-pool-name")]
    public string? InstancePoolName { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pid")]
    public string? Pid { get; set; }

    [CommandSwitch("--proxy-override")]
    public string? ProxyOverride { get; set; }

    [BooleanCommandSwitch("--public-data-endpoint-enabled")]
    public bool? PublicDataEndpointEnabled { get; set; }

    [CommandSwitch("--service-principal-type")]
    public string? ServicePrincipalType { get; set; }

    [CommandSwitch("--storage")]
    public string? Storage { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--timezone-id")]
    public string? TimezoneId { get; set; }

    [CommandSwitch("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}