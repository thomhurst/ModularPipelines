using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pin")]
public record PinOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--note")]
    public string? Note { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}