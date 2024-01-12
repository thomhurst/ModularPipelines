using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "scp")]
public record GcloudComputeScpOptions(
[property: PositionalArgument] string User
) : GcloudOptions
{
    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--plain")]
    public bool? Plain { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }

    [CommandSwitch("--scp-flag")]
    public string? ScpFlag { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--internal-ip")]
    public bool? InternalIp { get; set; }

    [BooleanCommandSwitch("--tunnel-through-iap")]
    public bool? TunnelThroughIap { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--dest-group")]
    public string? DestGroup { get; set; }

    [CommandSwitch("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CommandSwitch("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}