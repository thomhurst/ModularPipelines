using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "get")]
public record ConfigGetOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--value")]
    public virtual string? Value { get; set; }
}