using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "list")]
public record GcloudDatabaseMigrationConnectionProfilesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}