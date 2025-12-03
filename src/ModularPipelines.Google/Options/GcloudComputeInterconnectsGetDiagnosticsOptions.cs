using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "get-diagnostics")]
public record GcloudComputeInterconnectsGetDiagnosticsOptions(
[property: CliArgument] string Name
) : GcloudOptions;