using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "get-certificate-chain")]
public record GcloudKmsKeysVersionsGetCertificateChainOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsGetCertificateChainOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsGetCertificateChainOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsGetCertificateChainOptionsVersion { get; set; }

    [CommandSwitch("--certificate-chain-type")]
    public string? CertificateChainType { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }
}
