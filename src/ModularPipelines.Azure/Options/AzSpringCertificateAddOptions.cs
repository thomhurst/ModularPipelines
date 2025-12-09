using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "certificate", "add")]
public record AzSpringCertificateAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliFlag("--enable-auto-sync")]
    public bool? EnableAutoSync { get; set; }

    [CliFlag("--only-public-cert")]
    public bool? OnlyPublicCert { get; set; }

    [CliOption("--public-certificate-file")]
    public string? PublicCertificateFile { get; set; }

    [CliOption("--vault-certificate-name")]
    public string? VaultCertificateName { get; set; }

    [CliOption("--vault-uri")]
    public string? VaultUri { get; set; }
}