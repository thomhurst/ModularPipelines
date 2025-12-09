using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "session", "create")]
public record AzSynapseSparkSessionCreateOptions(
[property: CliOption("--executor-size")] string ExecutorSize,
[property: CliOption("--executors")] string Executors,
[property: CliOption("--name")] string Name,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--reference-files")]
    public string? ReferenceFiles { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}