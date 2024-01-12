using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "luns", "describe")]
public record GcloudBmsVolumesLunsDescribeOptions(
[property: PositionalArgument] string Lun,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Volume
) : GcloudOptions;