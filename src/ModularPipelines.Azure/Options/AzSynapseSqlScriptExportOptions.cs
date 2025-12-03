using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql-script", "export")]
public record AzSynapseSqlScriptExportOptions(
[property: CliOption("--output-folder")] string OutputFolder,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}