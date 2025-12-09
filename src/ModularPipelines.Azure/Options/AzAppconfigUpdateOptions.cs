using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "update")]
public record AzAppconfigUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliFlag("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CliFlag("--enable-purge-protection")]
    public bool? EnablePurgeProtection { get; set; }

    [CliOption("--encryption-key-name")]
    public string? EncryptionKeyName { get; set; }

    [CliOption("--encryption-key-vault")]
    public string? EncryptionKeyVault { get; set; }

    [CliOption("--encryption-key-version")]
    public string? EncryptionKeyVersion { get; set; }

    [CliOption("--identity-client-id")]
    public string? IdentityClientId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}