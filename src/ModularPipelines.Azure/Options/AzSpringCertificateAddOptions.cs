using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "certificate", "add")]
public record AzSpringCertificateAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--enable-auto-sync")]
    public bool? EnableAutoSync { get; set; }

    [BooleanCommandSwitch("--only-public-cert")]
    public bool? OnlyPublicCert { get; set; }

    [CommandSwitch("--public-certificate-file")]
    public string? PublicCertificateFile { get; set; }

    [CommandSwitch("--vault-certificate-name")]
    public string? VaultCertificateName { get; set; }

    [CommandSwitch("--vault-uri")]
    public string? VaultUri { get; set; }
}