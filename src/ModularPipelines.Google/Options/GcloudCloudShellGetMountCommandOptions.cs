using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-shell", "get-mount-command")]
public record GcloudCloudShellGetMountCommandOptions(
[property: CliArgument] string MountDir
) : GcloudOptions
{
    [CliFlag("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [CliFlag("--ssh-key-file")]
    public bool? SshKeyFile { get; set; }
}