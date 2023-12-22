using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "logout")]
public record YarnNpmLogoutOptions : YarnOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [BooleanCommandSwitch("--publish")]
    public bool? Publish { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}