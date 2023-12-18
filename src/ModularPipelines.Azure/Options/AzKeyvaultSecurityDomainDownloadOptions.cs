using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "security-domain", "download")]
public record AzKeyvaultSecurityDomainDownloadOptions(
[property: CommandSwitch("--sd-quorum")] string SdQuorum,
[property: CommandSwitch("--sd-wrapping-keys")] string SdWrappingKeys,
[property: CommandSwitch("--security-domain-file")] string SecurityDomainFile
) : AzOptions
{
    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}