using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "whoami")]
public record YarnNpmWhoamiOptions : YarnOptions
{
    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [BooleanCommandSwitch("--publish")]
    public virtual bool? Publish { get; set; }
}