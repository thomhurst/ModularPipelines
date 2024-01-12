using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "scp")]
public record GcloudComputeTpusTpuVmScpOptions(
[property: PositionalArgument] string User
) : GcloudOptions
{
    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--internal-ip")]
    public bool? InternalIp { get; set; }

    [BooleanCommandSwitch("--plain")]
    public bool? Plain { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }

    [CommandSwitch("--scp-flag")]
    public string? ScpFlag { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CommandSwitch("--worker")]
    public string? Worker { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CommandSwitch("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}