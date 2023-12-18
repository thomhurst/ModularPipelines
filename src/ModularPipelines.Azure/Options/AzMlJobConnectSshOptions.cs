using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "job", "connect-ssh")]
public record AzMlJobConnectSshOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--node-index")]
    public string? NodeIndex { get; set; }

    [CommandSwitch("--private-key-file-path")]
    public string? PrivateKeyFilePath { get; set; }
}