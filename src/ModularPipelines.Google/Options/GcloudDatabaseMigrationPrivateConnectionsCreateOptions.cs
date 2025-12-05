using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "private-connections", "create")]
public record GcloudDatabaseMigrationPrivateConnectionsCreateOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Region,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--subnet")] string Subnet,
[property: CliOption("--vpc")] string Vpc
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}