using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "session", "create")]
public record AzSynapseSparkSessionCreateOptions(
[property: CommandSwitch("--executor-size")] string ExecutorSize,
[property: CommandSwitch("--executors")] string Executors,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--spark-pool-name")] string SparkPoolName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--reference-files")]
    public string? ReferenceFiles { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

