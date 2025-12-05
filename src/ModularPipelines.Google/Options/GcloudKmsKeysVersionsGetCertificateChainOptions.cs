using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "get-certificate-chain")]
public record GcloudKmsKeysVersionsGetCertificateChainOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsGetCertificateChainOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsGetCertificateChainOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsGetCertificateChainOptionsVersion { get; set; }

    [CliOption("--certificate-chain-type")]
    public string? CertificateChainType { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }
}
