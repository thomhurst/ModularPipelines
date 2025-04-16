using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "logout")]
public record YarnNpmLogoutOptions : YarnOptions
{
    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [BooleanCommandSwitch("--publish")]
    public virtual bool? Publish { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }
}