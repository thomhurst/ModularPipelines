using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-shell", "scp")]
public record GcloudCloudShellScpOptions(
[property: CliArgument] string Cloudshell
) : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--recurse")]
    public bool? Recurse { get; set; }

    [CliOption("--scp-flag")]
    public string? ScpFlag { get; set; }

    [CliFlag("--ssh-key-file")]
    public bool? SshKeyFile { get; set; }
}