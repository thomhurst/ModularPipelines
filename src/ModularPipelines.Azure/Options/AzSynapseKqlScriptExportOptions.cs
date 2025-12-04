using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kql-script", "export")]
public record AzSynapseKqlScriptExportOptions(
[property: CliOption("--output-folder")] string OutputFolder,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}