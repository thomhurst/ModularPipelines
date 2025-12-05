using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark-job-definition", "show")]
public record AzSynapseSparkJobDefinitionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;