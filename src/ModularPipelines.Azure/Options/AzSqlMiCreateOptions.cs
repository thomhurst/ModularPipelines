using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "create")]
public record AzSqlMiCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-user")]
    public string? AdminUser { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliFlag("--enable-ad-only-auth")]
    public bool? EnableAdOnlyAuth { get; set; }

    [CliOption("--external-admin-name")]
    public string? ExternalAdminName { get; set; }

    [CliOption("--external-admin-principal-type")]
    public string? ExternalAdminPrincipalType { get; set; }

    [CliOption("--external-admin-sid")]
    public string? ExternalAdminSid { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--instance-pool-name")]
    public string? InstancePoolName { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pid")]
    public string? Pid { get; set; }

    [CliOption("--proxy-override")]
    public string? ProxyOverride { get; set; }

    [CliFlag("--public-data-endpoint-enabled")]
    public bool? PublicDataEndpointEnabled { get; set; }

    [CliOption("--service-principal-type")]
    public string? ServicePrincipalType { get; set; }

    [CliOption("--storage")]
    public string? Storage { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timezone-id")]
    public string? TimezoneId { get; set; }

    [CliOption("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}