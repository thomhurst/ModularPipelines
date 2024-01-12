using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "get-diagnostics")]
public record GcloudComputeInterconnectsGetDiagnosticsOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;