using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "spark", "statement", "show")]
public record AzSynapseSparkStatementShowOptions(
[property: CliOption("--livy-id")] string LivyId,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;