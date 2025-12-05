using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "describe")]
public record GcloudKmsKeysVersionsDescribeOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsDescribeOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsDescribeOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsDescribeOptionsVersion { get; set; }

    [CliOption("--attestation-file")]
    public string? AttestationFile { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
