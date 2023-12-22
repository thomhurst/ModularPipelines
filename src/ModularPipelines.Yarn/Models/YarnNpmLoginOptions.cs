using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "login")]
public record YarnNpmLoginOptions : YarnOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [BooleanCommandSwitch("--publish")]
    public bool? Publish { get; set; }

    [BooleanCommandSwitch("--always-auth")]
    public bool? AlwaysAuth { get; set; }
}