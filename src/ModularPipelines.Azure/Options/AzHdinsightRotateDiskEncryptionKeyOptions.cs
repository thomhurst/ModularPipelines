using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "rotate-disk-encryption-key")]
public record AzHdinsightRotateDiskEncryptionKeyOptions(
[property: CliOption("--encryption-key-name")] string EncryptionKeyName,
[property: CliOption("--encryption-key-version")] string EncryptionKeyVersion,
[property: CliOption("--encryption-vault-uri")] string EncryptionVaultUri,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}