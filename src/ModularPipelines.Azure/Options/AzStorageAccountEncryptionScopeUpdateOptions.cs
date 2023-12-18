using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "encryption-scope", "update")]
public record AzStorageAccountEncryptionScopeUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--key-source")]
    public string? KeySource { get; set; }

    [CommandSwitch("--key-uri")]
    public string? KeyUri { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}