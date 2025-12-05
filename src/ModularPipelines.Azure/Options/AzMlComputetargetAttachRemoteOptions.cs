using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "computetarget", "attach", "remote")]
public record AzMlComputetargetAttachRemoteOptions(
[property: CliOption("--address")] string Address,
[property: CliOption("--name")] string Name,
[property: CliOption("--ssh-port")] string SshPort,
[property: CliOption("--username")] string Username
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CliOption("--private-key-passphrase")]
    public string? PrivateKeyPassphrase { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}