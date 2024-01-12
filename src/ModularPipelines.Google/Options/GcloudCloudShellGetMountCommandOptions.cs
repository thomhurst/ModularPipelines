using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-shell", "get-mount-command")]
public record GcloudCloudShellGetMountCommandOptions(
[property: PositionalArgument] string MountDir
) : GcloudOptions
{
    [BooleanCommandSwitch("--force-key-file-overwrite")]
    public bool? ForceKeyFileOverwrite { get; set; }

    [BooleanCommandSwitch("--ssh-key-file")]
    public bool? SshKeyFile { get; set; }
}