using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "diagnose", "routes")]
public record GcloudComputeDiagnoseRoutesOptions(
[property: CliArgument] string Name,
[property: CliArgument] string TracerouteArgs
) : GcloudOptions
{
    [CliOption("--container")]
    public string? Container { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--external-route-ip")]
    public string? ExternalRouteIp { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--plain")]
    public bool? Plain { get; set; }

    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliFlag("--reverse-traceroute")]
    public bool? ReverseTraceroute { get; set; }

    [CliOption("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CliOption("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CliOption("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }

    [CliOption("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CliOption("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}