using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "statement", "invoke")]
public record AzSynapseSparkStatementInvokeOptions(
[property: CommandSwitch("--code")] string Code,
[property: CommandSwitch("--language")] string Language,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--spark-pool-name")] string SparkPoolName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;