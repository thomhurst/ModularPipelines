using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "diagnose", "routes")]
public record GcloudComputeDiagnoseRoutesOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string TracerouteArgs
) : GcloudOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--external-route-ip")]
    public string? ExternalRouteIp { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--plain")]
    public bool? Plain { get; set; }

    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [BooleanCommandSwitch("--reverse-traceroute")]
    public bool? ReverseTraceroute { get; set; }

    [CommandSwitch("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--zones")]
    public string[]? Zones { get; set; }

    [CommandSwitch("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CommandSwitch("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}