using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("login")]
public record NpmLoginOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--auth-type")]
    public virtual string? AuthType { get; set; }
}