using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "notebook", "export")]
public record AzSynapseNotebookExportOptions(
[property: CliOption("--output-folder")] string OutputFolder,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}