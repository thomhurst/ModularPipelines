using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kql-script", "import")]
public record AzSynapseKqlScriptImportOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--kusto-database-name")]
    public string? KustoDatabaseName { get; set; }

    [CliOption("--kusto-pool-name")]
    public string? KustoPoolName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}