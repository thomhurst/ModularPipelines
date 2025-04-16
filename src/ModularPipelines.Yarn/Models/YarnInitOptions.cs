using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("init")]
public record YarnInitOptions : YarnOptions
{
    [BooleanCommandSwitch("--private")]
    public virtual bool? Private { get; set; }

    [BooleanCommandSwitch("--workspace")]
    public virtual bool? Workspace { get; set; }

    [BooleanCommandSwitch("--install")]
    public virtual bool? Install { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }
}