using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databricks", "workspace", "update")]
public record AzDatabricksWorkspaceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-source")]
    public string? KeySource { get; set; }

    [CliOption("--key-vault")]
    public string? KeyVault { get; set; }

    [CliOption("--key-version")]
    public string? KeyVersion { get; set; }

    [CliOption("--managed-services-key-name")]
    public string? ManagedServicesKeyName { get; set; }

    [CliOption("--managed-services-key-vault")]
    public string? ManagedServicesKeyVault { get; set; }

    [CliOption("--managed-services-key-version")]
    public string? ManagedServicesKeyVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--prepare-encryption")]
    public bool? PrepareEncryption { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--required-nsg-rules")]
    public string? RequiredNsgRules { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sa-sku-name")]
    public string? SaSkuName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}