using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "routes", "create")]
public record GcloudDatastreamRoutesCreateOptions(
[property: PositionalArgument] string Route,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateConnection,
[property: CommandSwitch("--destination-address")] string DestinationAddress,
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [CommandSwitch("--destination-port")]
    public string? DestinationPort { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}