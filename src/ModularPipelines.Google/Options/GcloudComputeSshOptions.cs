using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "ssh")]
public record GcloudComputeSshOptions(
[property: CliArgument] string User,
[property: CliArgument] string SshArgs
) : GcloudOptions
{
    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--container")]
    public string? Container { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--plain")]
    public bool? Plain { get; set; }

    [CliOption("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CliFlag("--troubleshoot")]
    public bool? Troubleshoot { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CliFlag("--tunnel-through-iap")]
    public bool? TunnelThroughIap { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--dest-group")]
    public string? DestGroup { get; set; }

    [CliOption("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CliOption("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}