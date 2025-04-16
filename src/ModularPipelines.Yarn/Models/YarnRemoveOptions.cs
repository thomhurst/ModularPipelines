using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("remove")]
public record YarnRemoveOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--mode")]
    public virtual string? Mode { get; set; }
}