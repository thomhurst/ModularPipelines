using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace-package", "upload")]
public record AzSynapseWorkspacePackageUploadOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }
}