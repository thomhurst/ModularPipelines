using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-shell", "scp")]
public record GcloudCloudShellScpOptions(
[property: PositionalArgument] string Cloudshell
) : GcloudOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }

    [CommandSwitch("--scp-flag")]
    public string? ScpFlag { get; set; }

    [BooleanCommandSwitch("--ssh-key-file")]
    public bool? SshKeyFile { get; set; }
}