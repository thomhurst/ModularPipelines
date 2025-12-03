using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "update")]
public record GcloudKmsKeysVersionsUpdateOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsUpdateOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsUpdateOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsUpdateOptionsVersion { get; set; }

    [CliOption("--ekm-connection-key-path")]
    public string? EkmConnectionKeyPath { get; set; }

    [CliOption("--external-key-uri")]
    public string? ExternalKeyUri { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}
