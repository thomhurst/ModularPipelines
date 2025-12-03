using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "spark-job-definition", "list")]
public record AzSynapseSparkJobDefinitionListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;