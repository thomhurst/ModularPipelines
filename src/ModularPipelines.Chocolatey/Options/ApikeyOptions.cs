using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apikey")]
public record ApiKeyOptions : ChocoOptions
{
    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--api-key")]
    public virtual string? ApiKey { get; set; }
}