using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql-script", "export")]
public record AzSynapseSqlScriptExportOptions(
[property: CommandSwitch("--output-folder")] string OutputFolder,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}