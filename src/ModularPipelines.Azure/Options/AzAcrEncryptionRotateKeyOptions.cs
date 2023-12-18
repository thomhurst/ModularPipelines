using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "encryption", "rotate-key")]
public record AzAcrEncryptionRotateKeyOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--key-encryption-key")]
    public string? KeyEncryptionKey { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}