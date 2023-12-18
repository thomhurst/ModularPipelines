using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "certificate", "add")]
public record AzSphereDeviceCertificateAddOptions(
[property: CommandSwitch("--cert-type")] string CertType,
[property: CommandSwitch("--certificate")] string Certificate,
[property: CommandSwitch("--public-key-file")] string PublicKeyFile
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--private-key-password")]
    public string? PrivateKeyPassword { get; set; }
}