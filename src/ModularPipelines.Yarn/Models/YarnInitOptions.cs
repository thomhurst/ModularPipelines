using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("init")]
public record YarnInitOptions : YarnOptions
{
    [BooleanCommandSwitch("--private")]
    public bool? Private { get; set; }

    [BooleanCommandSwitch("--workspace")]
    public bool? Workspace { get; set; }

    [BooleanCommandSwitch("--install")]
    public bool? Install { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}