using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "encryption", "enable")]
public record AzVmssEncryptionEnableOptions(
[property: CommandSwitch("--disk-encryption-keyvault")] string DiskEncryptionKeyvault
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-encryption-algorithm")]
    public string? KeyEncryptionAlgorithm { get; set; }

    [CommandSwitch("--key-encryption-key")]
    public string? KeyEncryptionKey { get; set; }

    [CommandSwitch("--key-encryption-keyvault")]
    public string? KeyEncryptionKeyvault { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--volume-type")]
    public string? VolumeType { get; set; }
}

