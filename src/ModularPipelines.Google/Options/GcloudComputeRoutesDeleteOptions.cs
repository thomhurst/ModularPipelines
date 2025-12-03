using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routes", "delete")]
public record GcloudComputeRoutesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;