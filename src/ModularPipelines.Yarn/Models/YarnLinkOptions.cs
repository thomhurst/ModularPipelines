using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("link")]
public record YarnLinkOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--private")]
    public virtual bool? Private { get; set; }

    [BooleanCommandSwitch("--relative")]
    public virtual bool? Relative { get; set; }
}