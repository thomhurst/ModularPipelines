using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "rotate-disk-encryption-key")]
public record AzHdinsightRotateDiskEncryptionKeyOptions(
[property: CommandSwitch("--encryption-key-name")] string EncryptionKeyName,
[property: CommandSwitch("--encryption-key-version")] string EncryptionKeyVersion,
[property: CommandSwitch("--encryption-vault-uri")] string EncryptionVaultUri,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

