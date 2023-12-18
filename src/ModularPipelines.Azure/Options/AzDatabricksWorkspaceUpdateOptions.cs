using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databricks", "workspace", "update")]
public record AzDatabricksWorkspaceUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--key-source")]
    public string? KeySource { get; set; }

    [CommandSwitch("--key-vault")]
    public string? KeyVault { get; set; }

    [CommandSwitch("--key-version")]
    public string? KeyVersion { get; set; }

    [CommandSwitch("--managed-services-key-name")]
    public string? ManagedServicesKeyName { get; set; }

    [CommandSwitch("--managed-services-key-vault")]
    public string? ManagedServicesKeyVault { get; set; }

    [CommandSwitch("--managed-services-key-version")]
    public string? ManagedServicesKeyVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--prepare-encryption")]
    public bool? PrepareEncryption { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--required-nsg-rules")]
    public string? RequiredNsgRules { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sa-sku-name")]
    public string? SaSkuName { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

