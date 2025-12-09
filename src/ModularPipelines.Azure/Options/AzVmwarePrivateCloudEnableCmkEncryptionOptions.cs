using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "private-cloud", "enable-cmk-encryption")]
public record AzVmwarePrivateCloudEnableCmkEncryptionOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--enc-kv-key-name")]
    public string? EncKvKeyName { get; set; }

    [CliOption("--enc-kv-key-version")]
    public string? EncKvKeyVersion { get; set; }

    [CliOption("--enc-kv-url")]
    public string? EncKvUrl { get; set; }
}