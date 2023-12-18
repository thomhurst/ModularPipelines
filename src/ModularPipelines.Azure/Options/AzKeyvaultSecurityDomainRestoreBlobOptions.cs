using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "security-domain", "restore-blob")]
public record AzKeyvaultSecurityDomainRestoreBlobOptions(
[property: CommandSwitch("--sd-exchange-key")] string SdExchangeKey,
[property: CommandSwitch("--sd-file")] string SdFile,
[property: CommandSwitch("--sd-file-restore-blob")] string SdFileRestoreBlob,
[property: CommandSwitch("--sd-wrapping-keys")] string SdWrappingKeys
) : AzOptions
{
    [CommandSwitch("--passwords")]
    public string? Passwords { get; set; }
}

