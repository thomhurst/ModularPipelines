using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "encryption", "enable")]
public record AzVmssEncryptionEnableOptions(
[property: CliOption("--disk-encryption-keyvault")] string DiskEncryptionKeyvault
) : AzOptions
{
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