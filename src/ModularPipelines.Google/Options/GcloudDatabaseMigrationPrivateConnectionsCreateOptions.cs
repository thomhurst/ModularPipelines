using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "private-connections", "create")]
public record GcloudDatabaseMigrationPrivateConnectionsCreateOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--subnet")] string Subnet,
[property: CommandSwitch("--vpc")] string Vpc
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}