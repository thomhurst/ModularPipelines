using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "connect-to-serial-port")]
public record GcloudComputeConnectToSerialPortOptions(
[property: PositionalArgument] string User
) : GcloudOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--extra-args")]
    public IEnumerable<KeyValue>? ExtraArgs { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CommandSwitch("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}