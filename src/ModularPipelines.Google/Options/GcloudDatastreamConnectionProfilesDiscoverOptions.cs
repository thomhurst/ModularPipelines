using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "connection-profiles", "discover")]
public record GcloudDatastreamConnectionProfilesDiscoverOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--connection-profile-name")] string ConnectionProfileName,
[property: CliOption("--connection-profile-object-file")] string ConnectionProfileObjectFile
) : GcloudOptions
{
    [CliFlag("--full-hierarchy")]
    public bool? FullHierarchy { get; set; }

    [CliOption("--hierarchy-depth")]
    public string? HierarchyDepth { get; set; }

    [CliOption("--mysql-rdbms-file")]
    public string? MysqlRdbmsFile { get; set; }

    [CliOption("--oracle-rdbms-file")]
    public string? OracleRdbmsFile { get; set; }

    [CliOption("--postgresql-rdbms-file")]
    public string? PostgresqlRdbmsFile { get; set; }

    [CliFlag("--recursive")]
    public bool? Recursive { get; set; }

    [CliOption("--recursive-depth")]
    public string? RecursiveDepth { get; set; }
}