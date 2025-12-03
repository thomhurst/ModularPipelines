using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "spark-job-definition", "create")]
public record AzSynapseSparkJobDefinitionCreateOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--folder-path")]
    public string? FolderPath { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}