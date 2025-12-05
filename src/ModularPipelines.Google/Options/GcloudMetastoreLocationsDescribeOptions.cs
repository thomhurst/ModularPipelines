using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "locations", "describe")]
public record GcloudMetastoreLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;