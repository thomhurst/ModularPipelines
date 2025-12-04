using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "job", "show")]
public record AzSynapseSparkJobShowOptions(
[property: CliOption("--livy-id")] string LivyId,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;