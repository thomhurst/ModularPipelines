using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "set-primary-version")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Key { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Keyring { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }
}
