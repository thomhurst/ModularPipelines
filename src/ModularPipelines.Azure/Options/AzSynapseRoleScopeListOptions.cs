using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "role", "scope", "list")]
public record AzSynapseRoleScopeListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;