using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "pending", "merge")]
public record AzKeyvaultCertificatePendingMergeOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}