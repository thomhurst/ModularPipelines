using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "encryption-scope", "create")]
public record AzStorageAccountEncryptionScopeCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--key-source")]
    public string? KeySource { get; set; }

    [CommandSwitch("--key-uri")]
    public string? KeyUri { get; set; }

    [BooleanCommandSwitch("--require-infrastructure-encryption")]
    public bool? RequireInfrastructureEncryption { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}