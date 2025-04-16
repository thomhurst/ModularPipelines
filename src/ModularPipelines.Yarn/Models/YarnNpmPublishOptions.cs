using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "publish")]
public record YarnNpmPublishOptions : YarnOptions
{
    [CommandSwitch("--access")]
    public virtual string? Access { get; set; }

    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }

    [BooleanCommandSwitch("--tolerate-republish")]
    public virtual bool? TolerateRepublish { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }
}