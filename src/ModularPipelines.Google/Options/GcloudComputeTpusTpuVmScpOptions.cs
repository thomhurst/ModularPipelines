using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "scp")]
public record GcloudComputeTpusTpuVmScpOptions(
[property: CliArgument] string User
) : GcloudOptions
{
    [CliFlag("--compress")]
    public bool? Compress { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CliFlag("--plain")]
    public bool? Plain { get; set; }

    [CliFlag("--recurse")]
    public bool? Recurse { get; set; }

    [CliOption("--scp-flag")]
    public string? ScpFlag { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CliOption("--worker")]
    public string? Worker { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CliOption("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}