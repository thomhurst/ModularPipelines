using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "connection-profiles", "discover")]
public record GcloudDatastreamConnectionProfilesDiscoverOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--connection-profile-name")] string ConnectionProfileName,
[property: CommandSwitch("--connection-profile-object-file")] string ConnectionProfileObjectFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--full-hierarchy")]
    public bool? FullHierarchy { get; set; }

    [CommandSwitch("--hierarchy-depth")]
    public string? HierarchyDepth { get; set; }

    [CommandSwitch("--mysql-rdbms-file")]
    public string? MysqlRdbmsFile { get; set; }

    [CommandSwitch("--oracle-rdbms-file")]
    public string? OracleRdbmsFile { get; set; }

    [CommandSwitch("--postgresql-rdbms-file")]
    public string? PostgresqlRdbmsFile { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--recursive-depth")]
    public string? RecursiveDepth { get; set; }
}