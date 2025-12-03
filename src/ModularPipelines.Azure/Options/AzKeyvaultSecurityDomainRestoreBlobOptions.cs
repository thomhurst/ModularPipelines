using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "security-domain", "restore-blob")]
public record AzKeyvaultSecurityDomainRestoreBlobOptions(
[property: CliOption("--sd-exchange-key")] string SdExchangeKey,
[property: CliOption("--sd-file")] string SdFile,
[property: CliOption("--sd-file-restore-blob")] string SdFileRestoreBlob,
[property: CliOption("--sd-wrapping-keys")] string SdWrappingKeys
) : AzOptions
{
    [CliOption("--passwords")]
    public string? Passwords { get; set; }
}