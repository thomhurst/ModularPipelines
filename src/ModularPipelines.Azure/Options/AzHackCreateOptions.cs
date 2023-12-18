using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hack", "create")]
public record AzHackCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--runtime")] string Runtime
) : AzOptions
{
    [CommandSwitch("--ai")]
    public string? Ai { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }
}