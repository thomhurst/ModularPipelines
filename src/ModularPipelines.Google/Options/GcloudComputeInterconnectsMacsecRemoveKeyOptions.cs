using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "macsec", "remove-key")]
public record GcloudComputeInterconnectsMacsecRemoveKeyOptions(
[property: CliArgument] string Name,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions;