using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "session", "list")]
public record AzSynapseSparkSessionListOptions(
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--from-index")]
    public string? FromIndex { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}