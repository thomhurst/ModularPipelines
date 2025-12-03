using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "encryption", "enable")]
public record AzVmEncryptionEnableOptions(
[property: CliOption("--disk-encryption-keyvault")] string DiskEncryptionKeyvault
) : AzOptions
{
    [CliOption("--aad-client-cert-thumbprint")]
    public string? AadClientCertThumbprint { get; set; }

    [CliOption("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CliOption("--aad-client-secret")]
    public string? AadClientSecret { get; set; }

    [CliFlag("--encrypt-format-all")]
    public bool? EncryptFormatAll { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-encryption-algorithm")]
    public string? KeyEncryptionAlgorithm { get; set; }

    [CliOption("--key-encryption-key")]
    public string? KeyEncryptionKey { get; set; }

    [CliOption("--key-encryption-keyvault")]
    public string? KeyEncryptionKeyvault { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--volume-type")]
    public string? VolumeType { get; set; }
}