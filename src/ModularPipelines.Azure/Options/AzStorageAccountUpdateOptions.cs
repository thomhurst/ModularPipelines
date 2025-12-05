using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "update")]
public record AzStorageAccountUpdateOptions : AzOptions
{
    [CliOption("--access-tier")]
    public string? AccessTier { get; set; }

    [CliOption("--account-type")]
    public int? AccountType { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--allow-append")]
    public bool? AllowAppend { get; set; }

    [CliFlag("--allow-blob-public-access")]
    public bool? AllowBlobPublicAccess { get; set; }

    [CliFlag("--allow-cross-tenant-replication")]
    public bool? AllowCrossTenantReplication { get; set; }

    [CliFlag("--allow-shared-key-access")]
    public bool? AllowSharedKeyAccess { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--azure-storage-sid")]
    public string? AzureStorageSid { get; set; }

    [CliOption("--bypass")]
    public string? Bypass { get; set; }

    [CliOption("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--default-share-permission")]
    public string? DefaultSharePermission { get; set; }

    [CliOption("--domain-guid")]
    public string? DomainGuid { get; set; }

    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--domain-sid")]
    public string? DomainSid { get; set; }

    [CliFlag("--enable-files-aadds")]
    public bool? EnableFilesAadds { get; set; }

    [CliFlag("--enable-files-aadkerb")]
    public bool? EnableFilesAadkerb { get; set; }

    [CliFlag("--enable-files-adds")]
    public bool? EnableFilesAdds { get; set; }

    [CliFlag("--enable-large-file-share")]
    public bool? EnableLargeFileShare { get; set; }

    [CliFlag("--enable-local-user")]
    public bool? EnableLocalUser { get; set; }

    [CliFlag("--enable-sftp")]
    public bool? EnableSftp { get; set; }

    [CliOption("--encryption-key-name")]
    public string? EncryptionKeyName { get; set; }

    [CliOption("--encryption-key-source")]
    public string? EncryptionKeySource { get; set; }

    [CliOption("--encryption-key-vault")]
    public string? EncryptionKeyVault { get; set; }

    [CliOption("--encryption-key-version")]
    public string? EncryptionKeyVersion { get; set; }

    [CliOption("--encryption-services")]
    public string? EncryptionServices { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--forest-name")]
    public string? ForestName { get; set; }

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--immutability-period")]
    public string? ImmutabilityPeriod { get; set; }

    [CliOption("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CliOption("--key-exp-days")]
    public string? KeyExpDays { get; set; }

    [CliOption("--key-vault-federated-client-id")]
    public string? KeyVaultFederatedClientId { get; set; }

    [CliOption("--key-vault-user-identity-id")]
    public string? KeyVaultUserIdentityId { get; set; }

    [CliOption("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--net-bios-domain-name")]
    public string? NetBiosDomainName { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliFlag("--publish-internet-endpoints")]
    public bool? PublishInternetEndpoints { get; set; }

    [CliFlag("--publish-microsoft-endpoints")]
    public bool? PublishMicrosoftEndpoints { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--routing-choice")]
    public string? RoutingChoice { get; set; }

    [CliOption("--sam-account-name")]
    public int? SamAccountName { get; set; }

    [CliOption("--sas-exp")]
    public string? SasExp { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--use-subdomain")]
    public bool? UseSubdomain { get; set; }

    [CliOption("--user-identity-id")]
    public string? UserIdentityId { get; set; }
}