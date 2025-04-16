using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("star")]
public record NpmStarOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [BooleanCommandSwitch("--unicode")]
    public virtual bool? Unicode { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }
}