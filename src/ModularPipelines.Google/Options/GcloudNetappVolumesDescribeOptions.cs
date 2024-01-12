using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "describe")]
public record GcloudNetappVolumesDescribeOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Location
) : GcloudOptions;