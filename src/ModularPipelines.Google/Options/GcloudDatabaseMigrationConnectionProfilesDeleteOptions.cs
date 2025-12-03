using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "delete")]
public record GcloudDatabaseMigrationConnectionProfilesDeleteOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}