using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "locations", "describe")]
public record GcloudMetastoreLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;