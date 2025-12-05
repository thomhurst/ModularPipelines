using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "describe")]
public record GcloudBmsVolumesDescribeOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Region
) : GcloudOptions;