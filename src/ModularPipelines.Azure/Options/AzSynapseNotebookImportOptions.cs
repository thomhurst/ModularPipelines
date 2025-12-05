using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "notebook", "import")]
public record AzSynapseNotebookImportOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--executor-count")]
    public int? ExecutorCount { get; set; }

    [CliOption("--executor-size")]
    public string? ExecutorSize { get; set; }

    [CliOption("--folder-path")]
    public string? FolderPath { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--spark-pool-name")]
    public string? SparkPoolName { get; set; }
}