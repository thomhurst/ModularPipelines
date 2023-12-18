using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace-package", "upload-batch")]
public record AzSynapseWorkspacePackageUploadBatchOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }
}