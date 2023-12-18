using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "statement", "cancel")]
public record AzSynapseSparkStatementCancelOptions(
[property: CommandSwitch("--livy-id")] string LivyId,
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--spark-pool-name")] string SparkPoolName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}