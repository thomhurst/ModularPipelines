using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databricks", "workspace", "create")]
public record AzDatabricksWorkspaceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--disk-key-auto-rotation")]
    public bool? DiskKeyAutoRotation { get; set; }

    [CliOption("--disk-key-name")]
    public string? DiskKeyName { get; set; }

    [CliOption("--disk-key-vault")]
    public string? DiskKeyVault { get; set; }

    [CliOption("--disk-key-version")]
    public string? DiskKeyVersion { get; set; }

    [CliFlag("--enable-no-public-ip")]
    public bool? EnableNoPublicIp { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-resource-group")]
    public string? ManagedResourceGroup { get; set; }

    [CliOption("--managed-services-key-name")]
    public string? ManagedServicesKeyName { get; set; }

    [CliOption("--managed-services-key-vault")]
    public string? ManagedServicesKeyVault { get; set; }

    [CliOption("--managed-services-key-version")]
    public string? ManagedServicesKeyVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--prepare-encryption")]
    public bool? PrepareEncryption { get; set; }

    [CliOption("--private-subnet")]
    public string? PrivateSubnet { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--public-subnet")]
    public string? PublicSubnet { get; set; }

    [CliFlag("--require-infrastructure-encryption")]
    public bool? RequireInfrastructureEncryption { get; set; }

    [CliOption("--required-nsg-rules")]
    public string? RequiredNsgRules { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }
}