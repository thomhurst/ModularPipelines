using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "update")]
public record GcloudKmsKeysVersionsUpdateOptions : GcloudOptions
{
    public GcloudKmsKeysVersionsUpdateOptions(
        string version
    )
    {
        GcloudKmsKeysVersionsUpdateOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudKmsKeysVersionsUpdateOptionsVersion { get; set; }

    [CommandSwitch("--ekm-connection-key-path")]
    public string? EkmConnectionKeyPath { get; set; }

    [CommandSwitch("--external-key-uri")]
    public string? ExternalKeyUri { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--keyring")]
    public string? Keyring { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}
