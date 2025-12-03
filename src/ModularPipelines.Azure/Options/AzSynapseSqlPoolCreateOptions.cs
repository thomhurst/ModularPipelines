using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql", "pool", "create")]
public record AzSynapseSqlPoolCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--performance-level")] string PerformanceLevel,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}