using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("token", "list")]
public record NpmTokenListOptions : NpmOptions
{
    [BooleanCommandSwitch("--read-only")]
    public virtual bool? ReadOnly { get; set; }

    [CommandSwitch("--cidr")]
    public virtual string? Cidr { get; set; }

    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }
}