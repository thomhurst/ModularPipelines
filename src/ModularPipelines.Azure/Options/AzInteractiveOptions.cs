using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("interactive")]
public record AzInteractiveOptions(
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [CommandSwitch("--style")]
    public string? Style { get; set; }

    [CommandSwitch("--update")]
    public string? Update { get; set; }
}