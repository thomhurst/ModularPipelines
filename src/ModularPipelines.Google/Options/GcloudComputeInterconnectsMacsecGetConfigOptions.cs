using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "macsec", "get-config")]
public record GcloudComputeInterconnectsMacsecGetConfigOptions(
[property: CliArgument] string Name
) : GcloudOptions;