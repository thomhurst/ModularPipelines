using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "certificate", "add")]
public record AzSphereDeviceCertificateAddOptions(
[property: CliOption("--cert-type")] string CertType,
[property: CliOption("--certificate")] string Certificate,
[property: CliOption("--public-key-file")] string PublicKeyFile
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CliOption("--private-key-password")]
    public string? PrivateKeyPassword { get; set; }
}