using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "ssh")]
public record GcloudComputeTpusTpuVmSshOptions(
[property: CliArgument] string User,
[property: CliArgument] string SshArgs
) : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CliFlag("--plain")]
    public bool? Plain { get; set; }

    [CliOption("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CliOption("--worker")]
    public string? Worker { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CliOption("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CliOption("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}