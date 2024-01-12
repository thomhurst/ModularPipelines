using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "private-connections", "delete")]
public record GcloudDatastreamPrivateConnectionsDeleteOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}