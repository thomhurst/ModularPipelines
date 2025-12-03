using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "spark", "statement", "invoke")]
public record AzSynapseSparkStatementInvokeOptions(
[property: CliOption("--code")] string Code,
[property: CliOption("--language")] string Language,
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;