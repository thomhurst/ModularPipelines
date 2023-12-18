using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "key", "decrypt")]
public record AzKeyvaultKeyDecryptOptions(
[property: CommandSwitch("--algorithm")] string Algorithm,
[property: CommandSwitch("--value")] string Value
) : AzOptions
{
    [CommandSwitch("--aad")]
    public string? Aad { get; set; }

    [CommandSwitch("--data-type")]
    public string? DataType { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--iv")]
    public string? Iv { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}

