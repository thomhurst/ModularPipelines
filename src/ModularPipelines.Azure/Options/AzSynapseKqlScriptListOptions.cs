using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kql-script", "list")]
public record AzSynapseKqlScriptListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;