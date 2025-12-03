using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "scp")]
public record GcloudComputeScpOptions(
[property: CliArgument] string User
) : GcloudOptions
{
    [CliFlag("--compress")]
    public bool? Compress { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--plain")]
    public bool? Plain { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliFlag("--recurse")]
    public bool? Recurse { get; set; }

    [CliOption("--scp-flag")]
    public string? ScpFlag { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

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