using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "session", "show")]
public record AzSynapseSparkSessionShowOptions(
[property: CliOption("--livy-id")] string LivyId,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;