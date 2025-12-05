using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "dry-run", "describe")]
public record GcloudAccessContextManagerPerimetersDryRunDescribeOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy
) : GcloudOptions;