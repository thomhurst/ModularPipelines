using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "ssh")]
public record GcloudComputeTpusTpuVmSshOptions(
[property: PositionalArgument] string User,
[property: PositionalArgument] string SshArgs
) : GcloudOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--internal-ip")]
    public bool? InternalIp { get; set; }

    [BooleanCommandSwitch("--plain")]
    public bool? Plain { get; set; }

    [CommandSwitch("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CommandSwitch("--worker")]
    public string? Worker { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CommandSwitch("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}