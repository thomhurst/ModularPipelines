using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "show-usage")]
public record AzStorageAccountShowUsageOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--access-tier")]
    public string? AccessTier { get; set; }

    [CommandSwitch("--account-type")]
    public int? AccountType { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--allow-append")]
    public bool? AllowAppend { get; set; }

    [BooleanCommandSwitch("--allow-blob-public-access")]
    public bool? AllowBlobPublicAccess { get; set; }

    [BooleanCommandSwitch("--allow-cross-tenant-replication")]
    public bool? AllowCrossTenantReplication { get; set; }

    [BooleanCommandSwitch("--allow-shared-key-access")]
    public bool? AllowSharedKeyAccess { get; set; }

    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CommandSwitch("--azure-storage-sid")]
    public string? AzureStorageSid { get; set; }

    [CommandSwitch("--bypass")]
    public string? Bypass { get; set; }

    [CommandSwitch("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [CommandSwitch("--default-share-permission")]
    public string? DefaultSharePermission { get; set; }

    [CommandSwitch("--domain-guid")]
    public string? DomainGuid { get; set; }

    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--domain-sid")]
    public string? DomainSid { get; set; }

    [BooleanCommandSwitch("--enable-files-aadds")]
    public bool? EnableFilesAadds { get; set; }

    [BooleanCommandSwitch("--enable-files-aadkerb")]
    public bool? EnableFilesAadkerb { get; set; }

    [BooleanCommandSwitch("--enable-files-adds")]
    public bool? EnableFilesAdds { get; set; }

    [CommandSwitch("--enable-large-file-share")]
    public string? EnableLargeFileShare { get; set; }

    [BooleanCommandSwitch("--enable-local-user")]
    public bool? EnableLocalUser { get; set; }

    [BooleanCommandSwitch("--enable-sftp")]
    public bool? EnableSftp { get; set; }

    [CommandSwitch("--encryption-key-name")]
    public string? EncryptionKeyName { get; set; }

    [CommandSwitch("--encryption-key-source")]
    public string? EncryptionKeySource { get; set; }

    [CommandSwitch("--encryption-key-vault")]
    public string? EncryptionKeyVault { get; set; }

    [CommandSwitch("--encryption-key-version")]
    public string? EncryptionKeyVersion { get; set; }

    [CommandSwitch("--encryption-services")]
    public string? EncryptionServices { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--forest-name")]
    public string? ForestName { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--immutability-period")]
    public string? ImmutabilityPeriod { get; set; }

    [CommandSwitch("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CommandSwitch("--key-exp-days")]
    public string? KeyExpDays { get; set; }

    [CommandSwitch("--key-vault-federated-client-id")]
    public string? KeyVaultFederatedClientId { get; set; }

    [CommandSwitch("--key-vault-user-identity-id")]
    public string? KeyVaultUserIdentityId { get; set; }

    [CommandSwitch("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--net-bios-domain-name")]
    public string? NetBiosDomainName { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [BooleanCommandSwitch("--publish-internet-endpoints")]
    public bool? PublishInternetEndpoints { get; set; }

    [BooleanCommandSwitch("--publish-microsoft-endpoints")]
    public bool? PublishMicrosoftEndpoints { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--routing-choice")]
    public string? RoutingChoice { get; set; }

    [CommandSwitch("--sam-account-name")]
    public int? SamAccountName { get; set; }

    [CommandSwitch("--sas-exp")]
    public string? SasExp { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--use-subdomain")]
    public bool? UseSubdomain { get; set; }

    [CommandSwitch("--user-identity-id")]
    public string? UserIdentityId { get; set; }
}