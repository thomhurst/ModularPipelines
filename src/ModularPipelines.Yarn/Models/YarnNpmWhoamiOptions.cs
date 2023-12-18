using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "whoami")]
public record YarnNpmWhoamiOptions : YarnOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [BooleanCommandSwitch("--publish")]
    public bool? Publish { get; set; }
}