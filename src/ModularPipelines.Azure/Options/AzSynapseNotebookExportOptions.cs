using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "notebook", "export")]
public record AzSynapseNotebookExportOptions(
[property: CommandSwitch("--output-folder")] string OutputFolder,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}

