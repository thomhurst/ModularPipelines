using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "versions", "create")]
public record GcloudKmsKeysVersionsCreateOptions : GcloudOptions
{
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

    [BooleanCommandSwitch("--primary")]
    public bool? Primary { get; set; }
}