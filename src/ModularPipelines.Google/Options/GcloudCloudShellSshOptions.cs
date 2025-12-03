using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-shell", "ssh")]
public record GcloudCloudShellSshOptions : GcloudOptions
{
    [CliFlag("--authorize-session")]
    public bool? AuthorizeSession { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliOption("--ssh-flag")]
    public string? SshFlag { get; set; }

    [CliFlag("--ssh-key-file")]
    public bool? SshKeyFile { get; set; }
}