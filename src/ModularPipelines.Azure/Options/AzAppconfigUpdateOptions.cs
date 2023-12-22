using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "update")]
public record AzAppconfigUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [BooleanCommandSwitch("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [BooleanCommandSwitch("--enable-purge-protection")]
    public bool? EnablePurgeProtection { get; set; }

    [CommandSwitch("--encryption-key-name")]
    public string? EncryptionKeyName { get; set; }

    [CommandSwitch("--encryption-key-vault")]
    public string? EncryptionKeyVault { get; set; }

    [CommandSwitch("--encryption-key-version")]
    public string? EncryptionKeyVersion { get; set; }

    [CommandSwitch("--identity-client-id")]
    public string? IdentityClientId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}