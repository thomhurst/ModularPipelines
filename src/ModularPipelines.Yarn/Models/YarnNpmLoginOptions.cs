using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "login")]
public record YarnNpmLoginOptions : YarnOptions
{
    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [BooleanCommandSwitch("--publish")]
    public virtual bool? Publish { get; set; }

    [BooleanCommandSwitch("--always-auth")]
    public virtual bool? AlwaysAuth { get; set; }
}