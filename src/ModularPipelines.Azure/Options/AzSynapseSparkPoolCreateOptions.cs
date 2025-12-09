using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "pool", "create")]
public record AzSynapseSparkPoolCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--node-count")] int NodeCount,
[property: CliOption("--node-size")] string NodeSize,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--spark-version")] string SparkVersion,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--delay")]
    public string? Delay { get; set; }

    [CliFlag("--enable-auto-pause")]
    public bool? EnableAutoPause { get; set; }

    [CliFlag("--enable-auto-scale")]
    public bool? EnableAutoScale { get; set; }

    [CliFlag("--enable-dynamic-exec")]
    public bool? EnableDynamicExec { get; set; }

    [CliOption("--max-executors")]
    public string? MaxExecutors { get; set; }

    [CliOption("--max-node-count")]
    public int? MaxNodeCount { get; set; }

    [CliOption("--min-executors")]
    public string? MinExecutors { get; set; }

    [CliOption("--min-node-count")]
    public int? MinNodeCount { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-size-family")]
    public string? NodeSizeFamily { get; set; }

    [CliOption("--spark-config-file-path")]
    public string? SparkConfigFilePath { get; set; }

    [CliOption("--spark-events-folder")]
    public string? SparkEventsFolder { get; set; }

    [CliOption("--spark-log-folder")]
    public string? SparkLogFolder { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}