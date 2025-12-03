using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routes", "describe")]
public record GcloudComputeRoutesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;