using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "security-domain", "upload")]
public record AzKeyvaultSecurityDomainUploadOptions(
[property: CliOption("--sd-file")] string SdFile
) : AzOptions
{
    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--passwords")]
    public string? Passwords { get; set; }

    [CliFlag("--restore-blob")]
    public bool? RestoreBlob { get; set; }

    [CliOption("--sd-exchange-key")]
    public string? SdExchangeKey { get; set; }

    [CliOption("--sd-wrapping-keys")]
    public string? SdWrappingKeys { get; set; }
}