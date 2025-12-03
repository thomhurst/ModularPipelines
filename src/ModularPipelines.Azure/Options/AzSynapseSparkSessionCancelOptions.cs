using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "spark", "session", "cancel")]
public record AzSynapseSparkSessionCancelOptions(
[property: CliOption("--livy-id")] string LivyId,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}