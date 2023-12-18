using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "attach", "remote")]
public record AzMlComputetargetAttachRemoteOptions(
[property: CommandSwitch("--address")] string Address,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--ssh-port")] string SshPort,
[property: CommandSwitch("--username")] string Username
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--private-key-passphrase")]
    public string? PrivateKeyPassphrase { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}

