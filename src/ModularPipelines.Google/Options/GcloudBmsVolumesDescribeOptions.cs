using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "describe")]
public record GcloudBmsVolumesDescribeOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Region
) : GcloudOptions;