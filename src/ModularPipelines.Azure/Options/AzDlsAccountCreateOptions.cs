using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dls", "account", "create")]
public record AzDlsAccountCreateOptions(
[property: CliOption("--account")] int Account
) : AzOptions
{
    [CliOption("--default-group")]
    public string? DefaultGroup { get; set; }

    [CliFlag("--disable-encryption")]
    public bool? DisableEncryption { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-vault-id")]
    public string? KeyVaultId { get; set; }

    [CliOption("--key-version")]
    public string? KeyVersion { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }
}