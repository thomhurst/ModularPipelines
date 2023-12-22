using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("link")]
public record YarnLinkOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--private")]
    public bool? Private { get; set; }

    [BooleanCommandSwitch("--relative")]
    public bool? Relative { get; set; }
}