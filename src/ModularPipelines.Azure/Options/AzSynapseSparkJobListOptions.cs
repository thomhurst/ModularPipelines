using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "job", "list")]
public record AzSynapseSparkJobListOptions(
[property: CommandSwitch("--spark-pool-name")] string SparkPoolName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--from-index")]
    public string? FromIndex { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}