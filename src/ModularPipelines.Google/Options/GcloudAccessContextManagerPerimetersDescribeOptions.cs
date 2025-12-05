using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "describe")]
public record GcloudAccessContextManagerPerimetersDescribeOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy
) : GcloudOptions;