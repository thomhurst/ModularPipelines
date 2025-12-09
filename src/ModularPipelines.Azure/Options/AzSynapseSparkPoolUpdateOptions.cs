using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "spark", "pool", "update")]
public record AzSynapseSparkPoolUpdateOptions : AzOptions
{
    [CliOption("--delay")]
    public string? Delay { get; set; }

    [CliFlag("--enable-auto-pause")]
    public bool? EnableAutoPause { get; set; }

    [CliFlag("--enable-auto-scale")]
    public bool? EnableAutoScale { get; set; }

    [CliFlag("--enable-dynamic-exec")]
    public bool? EnableDynamicExec { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--library-requirements")]
    public string? LibraryRequirements { get; set; }

    [CliOption("--max-executors")]
    public string? MaxExecutors { get; set; }

    [CliOption("--max-node-count")]
    public int? MaxNodeCount { get; set; }

    [CliOption("--min-executors")]
    public string? MinExecutors { get; set; }

    [CliOption("--min-node-count")]
    public int? MinNodeCount { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--node-size")]
    public string? NodeSize { get; set; }

    [CliOption("--package")]
    public string? Package { get; set; }

    [CliOption("--package-action")]
    public string? PackageAction { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--spark-config-file-path")]
    public string? SparkConfigFilePath { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}