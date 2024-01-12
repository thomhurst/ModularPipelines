using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "ssh")]
public record GcloudComputeSshOptions(
[property: PositionalArgument] string User,
[property: PositionalArgument] string SshArgs
) : GcloudOptions
{
    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--plain")]
    public bool? Plain { get; set; }

    [CommandSwitch("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [BooleanCommandSwitch("--troubleshoot")]
    public bool? Troubleshoot { get; set; }

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