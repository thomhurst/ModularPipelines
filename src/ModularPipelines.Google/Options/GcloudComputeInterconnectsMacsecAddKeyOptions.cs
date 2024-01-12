using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "macsec", "add-key")]
public record GcloudComputeInterconnectsMacsecAddKeyOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions
{
    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}