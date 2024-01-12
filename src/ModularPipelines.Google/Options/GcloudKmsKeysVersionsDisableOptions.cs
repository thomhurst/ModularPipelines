using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "disable")]
public record GcloudKmsKeysVersionsDisableOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsDisableOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsDisableOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsDisableOptionsVersion { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
