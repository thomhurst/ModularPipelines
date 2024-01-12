using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "macsec", "remove-key")]
public record GcloudComputeInterconnectsMacsecRemoveKeyOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions;