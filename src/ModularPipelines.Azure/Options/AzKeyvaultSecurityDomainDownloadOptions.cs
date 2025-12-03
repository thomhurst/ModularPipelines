using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "security-domain", "download")]
public record AzKeyvaultSecurityDomainDownloadOptions(
[property: CliOption("--sd-quorum")] string SdQuorum,
[property: CliOption("--sd-wrapping-keys")] string SdWrappingKeys,
[property: CliOption("--security-domain-file")] string SecurityDomainFile
) : AzOptions
{
    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}