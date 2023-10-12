using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("unlink")]
public record YarnUnlinkOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}