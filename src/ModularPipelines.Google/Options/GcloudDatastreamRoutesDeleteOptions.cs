using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "routes", "delete")]
public record GcloudDatastreamRoutesDeleteOptions(
[property: PositionalArgument] string Route,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateConnection
) : GcloudOptions;