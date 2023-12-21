using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "pool", "update")]
public record AzSynapseSparkPoolUpdateOptions : AzOptions
{
    [CommandSwitch("--delay")]
    public string? Delay { get; set; }

    [BooleanCommandSwitch("--enable-auto-pause")]
    public bool? EnableAutoPause { get; set; }

    [BooleanCommandSwitch("--enable-auto-scale")]
    public bool? EnableAutoScale { get; set; }

    [BooleanCommandSwitch("--enable-dynamic-exec")]
    public bool? EnableDynamicExec { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--library-requirements")]
    public string? LibraryRequirements { get; set; }

    [CommandSwitch("--max-executors")]
    public string? MaxExecutors { get; set; }

    [CommandSwitch("--max-node-count")]
    public int? MaxNodeCount { get; set; }

    [CommandSwitch("--min-executors")]
    public string? MinExecutors { get; set; }

    [CommandSwitch("--min-node-count")]
    public int? MinNodeCount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-size")]
    public string? NodeSize { get; set; }

    [CommandSwitch("--package")]
    public string? Package { get; set; }

    [CommandSwitch("--package-action")]
    public string? PackageAction { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--spark-config-file-path")]
    public string? SparkConfigFilePath { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}