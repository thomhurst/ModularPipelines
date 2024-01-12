using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "copy-files")]
public record GcloudComputeCopyFilesOptions(
[property: PositionalArgument] string User
) : GcloudOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--plain")]
    public bool? Plain { get; set; }

    [CommandSwitch("--ssh-key-file")]
    public string? SshKeyFile { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--ssh-key-expiration")]
    public string? SshKeyExpiration { get; set; }

    [CommandSwitch("--ssh-key-expire-after")]
    public string? SshKeyExpireAfter { get; set; }
}