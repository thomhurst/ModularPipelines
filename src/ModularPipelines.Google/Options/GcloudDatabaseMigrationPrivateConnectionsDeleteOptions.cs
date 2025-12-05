using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "private-connections", "delete")]
public record GcloudDatabaseMigrationPrivateConnectionsDeleteOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }
}