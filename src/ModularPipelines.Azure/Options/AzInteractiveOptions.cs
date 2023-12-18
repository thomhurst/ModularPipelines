using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("interactive")]
public record AzInteractiveOptions : AzOptions
{
    [CommandSwitch("--style")]
    public string? Style { get; set; }

    [CommandSwitch("--update")]
    public string? Update { get; set; }
}