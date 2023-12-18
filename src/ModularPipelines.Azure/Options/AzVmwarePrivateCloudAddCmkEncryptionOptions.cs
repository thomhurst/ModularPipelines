using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-cloud", "add-cmk-encryption")]
public record AzVmwarePrivateCloudAddCmkEncryptionOptions(
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--enc-kv-key-name")]
    public string? EncKvKeyName { get; set; }

    [CommandSwitch("--enc-kv-key-version")]
    public string? EncKvKeyVersion { get; set; }

    [CommandSwitch("--enc-kv-url")]
    public string? EncKvUrl { get; set; }
}