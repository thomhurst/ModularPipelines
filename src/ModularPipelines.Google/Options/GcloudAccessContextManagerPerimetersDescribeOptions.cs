using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "describe")]
public record GcloudAccessContextManagerPerimetersDescribeOptions(
[property: PositionalArgument] string Perimeter,
[property: PositionalArgument] string Policy
) : GcloudOptions;