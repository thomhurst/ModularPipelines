using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "macsec", "update-key")]
public record GcloudComputeInterconnectsMacsecUpdateKeyOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions
{
    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}