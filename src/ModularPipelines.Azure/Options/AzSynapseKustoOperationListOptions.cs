using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kusto-operation", "list")]
public record AzSynapseKustoOperationListOptions : AzOptions;