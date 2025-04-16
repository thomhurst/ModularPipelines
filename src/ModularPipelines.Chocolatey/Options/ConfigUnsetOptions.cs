using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "unset")]
public record ConfigUnsetOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--value")]
    public virtual string? Value { get; set; }
}