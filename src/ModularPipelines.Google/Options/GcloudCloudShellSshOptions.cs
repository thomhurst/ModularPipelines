using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-shell", "ssh")]
public record GcloudCloudShellSshOptions : GcloudOptions
{
    [BooleanCommandSwitch("--authorize-session")]
    public bool? AuthorizeSession { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CommandSwitch("--ssh-flag")]
    public string? SshFlag { get; set; }

    [BooleanCommandSwitch("--ssh-key-file")]
    public bool? SshKeyFile { get; set; }
}