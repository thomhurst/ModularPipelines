using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databricks", "workspace", "create")]
public record AzDatabricksWorkspaceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--disk-key-auto-rotation")]
    public bool? DiskKeyAutoRotation { get; set; }

    [CommandSwitch("--disk-key-name")]
    public string? DiskKeyName { get; set; }

    [CommandSwitch("--disk-key-vault")]
    public string? DiskKeyVault { get; set; }

    [CommandSwitch("--disk-key-version")]
    public string? DiskKeyVersion { get; set; }

    [BooleanCommandSwitch("--enable-no-public-ip")]
    public bool? EnableNoPublicIp { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-resource-group")]
    public string? ManagedResourceGroup { get; set; }

    [CommandSwitch("--managed-services-key-name")]
    public string? ManagedServicesKeyName { get; set; }

    [CommandSwitch("--managed-services-key-vault")]
    public string? ManagedServicesKeyVault { get; set; }

    [CommandSwitch("--managed-services-key-version")]
    public string? ManagedServicesKeyVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--prepare-encryption")]
    public bool? PrepareEncryption { get; set; }

    [CommandSwitch("--private-subnet")]
    public string? PrivateSubnet { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--public-subnet")]
    public string? PublicSubnet { get; set; }

    [BooleanCommandSwitch("--require-infrastructure-encryption")]
    public bool? RequireInfrastructureEncryption { get; set; }

    [CommandSwitch("--required-nsg-rules")]
    public string? RequiredNsgRules { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }
}