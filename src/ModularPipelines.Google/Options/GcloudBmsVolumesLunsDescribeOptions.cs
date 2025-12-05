using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "luns", "describe")]
public record GcloudBmsVolumesLunsDescribeOptions(
[property: CliArgument] string Lun,
[property: CliArgument] string Region,
[property: CliArgument] string Volume
) : GcloudOptions;