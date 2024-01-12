using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "macsec", "get-config")]
public record GcloudComputeInterconnectsMacsecGetConfigOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;