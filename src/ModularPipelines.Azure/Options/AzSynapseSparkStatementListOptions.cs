using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "statement", "list")]
public record AzSynapseSparkStatementListOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;