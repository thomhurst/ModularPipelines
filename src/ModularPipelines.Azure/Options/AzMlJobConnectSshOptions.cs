using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "job", "connect-ssh")]
public record AzMlJobConnectSshOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--node-index")]
    public string? NodeIndex { get; set; }

    [CliOption("--private-key-file-path")]
    public string? PrivateKeyFilePath { get; set; }
}