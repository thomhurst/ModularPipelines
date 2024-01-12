using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "delete")]
public record GcloudDatabaseMigrationConnectionProfilesDeleteOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}