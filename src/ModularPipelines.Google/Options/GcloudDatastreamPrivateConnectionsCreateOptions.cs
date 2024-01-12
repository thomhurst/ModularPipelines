using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "private-connections", "create")]
public record GcloudDatastreamPrivateConnectionsCreateOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--subnet")] string Subnet,
[property: CommandSwitch("--vpc")] string Vpc
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}