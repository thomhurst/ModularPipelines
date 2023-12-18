using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "notebook", "list")]
public record AzSynapseNotebookListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--executor-count")]
    public int? ExecutorCount { get; set; }

    [CommandSwitch("--executor-size")]
    public string? ExecutorSize { get; set; }

    [CommandSwitch("--folder-path")]
    public string? FolderPath { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--spark-pool-name")]
    public string? SparkPoolName { get; set; }
}