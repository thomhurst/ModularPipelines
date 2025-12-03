using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "database", "create", "(kusto", "extension)")]
public record AzKustoDatabaseCreateKustoExtensionOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--read-only-following-database")]
    public string? ReadOnlyFollowingDatabase { get; set; }

    [CliOption("--read-write-database")]
    public string? ReadWriteDatabase { get; set; }
}