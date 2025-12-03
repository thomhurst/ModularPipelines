using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "get-public-key")]
public record GcloudKmsKeysVersionsGetPublicKeyOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsGetPublicKeyOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsGetPublicKeyOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsGetPublicKeyOptionsVersion { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }
}
