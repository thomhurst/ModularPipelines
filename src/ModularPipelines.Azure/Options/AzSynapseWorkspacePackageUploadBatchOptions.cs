using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "workspace-package", "upload-batch")]
public record AzSynapseWorkspacePackageUploadBatchOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }
}