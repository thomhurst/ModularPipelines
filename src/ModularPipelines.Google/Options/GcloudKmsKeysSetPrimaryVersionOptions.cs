using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "set-primary-version")]
public record GcloudKmsKeysSetPrimaryVersionOptions : GcloudOptions
{
    public GcloudKmsKeysSetPrimaryVersionOptions(
        string key,
        string keyring,
        string location,
        string version
    )
    {
        Key = key;
        Keyring = keyring;
        Location = location;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Key { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Keyring { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }
}
