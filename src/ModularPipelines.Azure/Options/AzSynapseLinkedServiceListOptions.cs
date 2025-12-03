using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "linked-service", "list")]
public record AzSynapseLinkedServiceListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;