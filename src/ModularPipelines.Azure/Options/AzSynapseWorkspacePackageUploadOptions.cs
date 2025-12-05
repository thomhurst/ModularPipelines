using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "workspace-package", "upload")]
public record AzSynapseWorkspacePackageUploadOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }
}