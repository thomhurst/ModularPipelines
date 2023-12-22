using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("unstar")]
public record NpmUnstarOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [BooleanCommandSwitch("--unicode")]
    public bool? Unicode { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }
}