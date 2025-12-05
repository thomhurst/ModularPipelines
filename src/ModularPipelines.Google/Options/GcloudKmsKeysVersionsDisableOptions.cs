using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "versions", "disable")]
public record GcloudKmsKeysVersionsDisableOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsDisableOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsDisableOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudKmsKeysVersionsDisableOptionsVersion { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
