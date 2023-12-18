using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "pool", "create")]
public record AzSynapseSparkPoolCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--node-count")] int NodeCount,
[property: CommandSwitch("--node-size")] string NodeSize,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--spark-version")] string SparkVersion,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--delay")]
    public string? Delay { get; set; }

    [BooleanCommandSwitch("--enable-auto-pause")]
    public bool? EnableAutoPause { get; set; }

    [BooleanCommandSwitch("--enable-auto-scale")]
    public bool? EnableAutoScale { get; set; }

    [BooleanCommandSwitch("--enable-dynamic-exec")]
    public bool? EnableDynamicExec { get; set; }

    [CommandSwitch("--max-executors")]
    public string? MaxExecutors { get; set; }

    [CommandSwitch("--max-node-count")]
    public int? MaxNodeCount { get; set; }

    [CommandSwitch("--min-executors")]
    public string? MinExecutors { get; set; }

    [CommandSwitch("--min-node-count")]
    public int? MinNodeCount { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-size-family")]
    public string? NodeSizeFamily { get; set; }

    [CommandSwitch("--spark-config-file-path")]
    public string? SparkConfigFilePath { get; set; }

    [CommandSwitch("--spark-events-folder")]
    public string? SparkEventsFolder { get; set; }

    [CommandSwitch("--spark-log-folder")]
    public string? SparkLogFolder { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

