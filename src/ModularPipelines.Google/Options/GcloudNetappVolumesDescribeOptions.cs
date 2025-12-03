using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "describe")]
public record GcloudNetappVolumesDescribeOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Location
) : GcloudOptions;