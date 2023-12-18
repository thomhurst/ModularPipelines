using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "security-domain", "upload")]
public record AzKeyvaultSecurityDomainUploadOptions(
[property: CommandSwitch("--sd-file")] string SdFile
) : AzOptions
{
    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--passwords")]
    public string? Passwords { get; set; }

    [BooleanCommandSwitch("--restore-blob")]
    public bool? RestoreBlob { get; set; }

    [CommandSwitch("--sd-exchange-key")]
    public string? SdExchangeKey { get; set; }

    [CommandSwitch("--sd-wrapping-keys")]
    public string? SdWrappingKeys { get; set; }
}