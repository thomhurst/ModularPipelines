using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "workspace-package", "list")]
public record AzSynapseWorkspacePackageListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;